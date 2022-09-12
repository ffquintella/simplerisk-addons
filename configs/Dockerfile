# Using Ubuntu kinetic image
FROM ubuntu:jammy

# Maintained by SimpleRisk
LABEL maintainer="ffquintella"

# Make necessary directories
RUN mkdir -p /configurations /etc/apache2/ssl /passwords /var/log /var/log/lib/mysql /var/log/www/simplerisk /srnet
                                                                    
# Installing apt dependencies     
RUN dpkg-divert --local --rename /usr/bin/ischroot && \
    ln -sf /bin/true /usr/bin/ischroot && \
    apt-get update && \
    apt-get upgrade -y && \
    DEBIAN_FRONTEND=noninteractive apt-get -y install apache2 \
                                                      php \
                                                      php-mysql \
                                                      php-json \
                                                      mariadb-client\
                                                      mariadb-server \
                                                      php-dev \
                                                      php-ldap \
                                                      php-mbstring \
                                                      php-curl \
                                                      php-zip \
                                                      php-gd \
                                                      nfs-common \
                                                      chrony \
                                                      cron \
                                                      python-setuptools \
                                                      vim \
                                                      sendmail \
                                                      openssl \
                                                      ufw \
                                                      sudo \
                                                      fish \
                                                      puppet \
                                                      supervisor && \
    rm -rf /var/lib/apt/lists

# Create the OpenSSL password
RUN < /dev/urandom tr -dc _A-Z-a-z-0-9 | head -c21 > /passwords/pass_openssl.txt

# Install and configure supervisor
COPY common/supervisord.conf /etc/supervisord.conf
#RUN mkdir /var/log/supervisor/
RUN service supervisor restart

# Configure MySQL
#RUN sed -i 's/\[mysqld\]/\[mysqld\]\nsql-mode="NO_ENGINE_SUBSTITUTION"/g' /etc/mysql/mysql.conf.d/mysqld.cnf
RUN sed -i 's/\[mysqld\]/\[mysqld\]\ninnodb-use-native-aio=0/g' /etc/mysql/mariadb.conf.d/50-server.cnf
RUN sed -i 's/\[mysqld\]/\[mysqld\]\nsql-mode="NO_ENGINE_SUBSTITUTION"/g' /etc/mysql/mariadb.conf.d/50-server.cnf


# Configure Apache
COPY common/foreground.sh /etc/apache2/foreground.sh
RUN chmod 755 /etc/apache2/foreground.sh
COPY common/envvars /etc/apache2/envvars
COPY common/000-default.conf /etc/apache2/sites-enabled/000-default.conf
COPY common/default-ssl.conf /etc/apache2/sites-enabled/default-ssl.conf
#RUN sed -i 's/upload_max_filesize = 2M/upload_max_filesize = 50M/g' /etc/php/8.1/apache2/php.ini
RUN sed -i 's/\(upload_max_filesize =\) .*\(M\)/\1 50\2/g' /etc/php/8.1/apache2/php.ini
RUN sed -i 's/\(post_max_size =\) .*\(M\)/\1 50\2/g' /etc/php/8.1/apache2/php.ini
RUN sed -i 's/\(memory_limit =\) .*\(M\)/\1 256\2/g' /etc/php/8.1/apache2/php.ini
RUN sed -i '/max_input_vars = 1000/a max_input_vars = 3000' /etc/php/8.1/apache2/php.ini

# Create SSL Certificates for Apache SSL
RUN mkdir -p /etc/apache2/ssl/ssl.crt
RUN mkdir -p /etc/apache2/ssl/ssl.key
RUN openssl genrsa -des3 -passout pass:/passwords/pass_openssl.txt -out /etc/apache2/ssl/ssl.key/simplerisk.pass.key
RUN openssl rsa -passin pass:/passwords/pass_openssl.txt -in /etc/apache2/ssl/ssl.key/simplerisk.pass.key -out /etc/apache2/ssl/ssl.key/simplerisk.key
RUN rm /etc/apache2/ssl/ssl.key/simplerisk.pass.key
RUN openssl req -new -key /etc/apache2/ssl/ssl.key/simplerisk.key -out  /etc/apache2/ssl/ssl.crt/simplerisk.csr -subj "/CN=simplerisk"
RUN openssl x509 -req -days 365 -in /etc/apache2/ssl/ssl.crt/simplerisk.csr -signkey /etc/apache2/ssl/ssl.key/simplerisk.key -out /etc/apache2/ssl/ssl.crt/simplerisk.crt

# Activate Apache modules
RUN phpenmod ldap
RUN a2enmod rewrite
RUN a2enmod ssl
RUN a2enconf security
RUN sed -i 's/SSLProtocol all -SSLv3/SSLProtocol TLSv1.2/g' /etc/apache2/mods-enabled/ssl.conf
RUN sed -i 's/#SSLHonorCipherOrder on/SSLHonorCipherOrder on/g' /etc/apache2/mods-enabled/ssl.conf
RUN sed -i 's/ServerTokens OS/ServerTokens Prod/g' /etc/apache2/conf-enabled/security.conf
RUN sed -i 's/ServerSignature On/ServerSignature Off/g' /etc/apache2/conf-enabled/security.conf

RUN echo %sudo  ALL=NOPASSWD: ALL >> /etc/sudoers && \
    echo "20220401-001" > /tmp/version

# COPY SRNET
COPY srnet/ /srnet
COPY srnet-db/ /scripts/srnet-db
COPY srnet-db/srnetdb.version /configurations/srnetdb.version

# COPY Simple risk
WORKDIR /var/www/
COPY app/simplerisk /var/www/simplerisk
COPY simplerisk.sql /scripts/db/simplerisk.sql


RUN mkdir -p /scripts/dbmgmt/ 
COPY restore_db.sh /scripts/dbmgmt/restore_db.sh
RUN chmod +x /scripts/dbmgmt/restore_db.sh

# Permissions
RUN chown -R www-data:www-data /var/www/simplerisk && chown -R :www-data /var/log/apache2 && chmod g+rwx /var/log/apache2

# Setting up cronjob
RUN echo "* * * * * /usr/bin/php -f /var/www/simplerisk/cron/cron.php > /dev/null 2>&1" >> /etc/cron.d/backup-cron && \
    chmod 0644 /etc/cron.d/backup-cron && \
    crontab /etc/cron.d/backup-cron


# Puppet stuff 
ENV BASE_HOST "127.0.0.1"
ENV BASE_URL "https://${BASE_HOST}/"

ENV FACTER_BASE_HOST $BASE_HOST
ENV FACTER_BASE_URL $BASE_URL
ENV FACTER_SRNET_URL "https://${BASE_HOST}:7072/"


RUN  mkdir /etc/puppet/manifests ; mkdir /etc/puppet/modules ; mkdir /opt/scripts
COPY puppet/manifests /etc/puppet/manifests/
COPY puppet/modules /etc/puppet/modules/
RUN /usr/bin/puppet apply --modulepath=/etc/puppet/modules /etc/puppet/manifests/base.pp ; \
    yum clean all ; rm -rf /tmp/* ; rm -rf /var/cache/* ; rm -rf /var/tmp/* ; rm -rf /var/opt/staging


EXPOSE 80
EXPOSE 443
EXPOSE 7072

# Create the start script and set permissions
COPY common/entrypoint.sh /entrypoint.sh
RUN chmod 755 /entrypoint.sh

# Data to save
VOLUME /passwords
VOLUME /configurations
VOLUME /var/log
VOLUME /var/lib/mysql
VOLUME /etc/apache2/ssl
VOLUME /var/www/simplerisk

# Setting up entrypoint
ENTRYPOINT [ "/entrypoint.sh" ]

# Start Apache and MySQL
CMD ["/usr/sbin/apache2ctl", "-D", "FOREGROUND"]


