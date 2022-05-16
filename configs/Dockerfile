FROM php:8.1-apache

WORKDIR /var/www/html

COPY app/simplerisk/* ./

COPY sr-apache-site.conf /etc/apache2/sites-available/sr-apache-site.conf

RUN echo "ServerName localhost" >> /etc/apache2/apache2.conf && a2enmod rewrite && a2dissite 000-default && a2ensite my-apache-site && service apache2 restart

RUN docker-php-ext-configure gd --with-jpeg=/usr/include/ && docker-php-ext-install gd
RUN docker-php-ext-install pdo_mysql

EXPOSE 80
EXPOSE 443
