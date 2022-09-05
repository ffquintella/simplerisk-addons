﻿using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Model.Exceptions;

namespace ServerServices;
using ILogger = Serilog.ILogger;


public class AssessmentsService: ServiceBase, IAssessmentsService
{
    
    public AssessmentsService(ILogger logger, DALManager dalManager): base(logger, dalManager)
    {
        
    }
    
    public List<Assessment> List()
    {
        var srDbContext = DALManager.GetContext();
        
        return srDbContext.Assessments.ToList();
        
    }

    public Assessment? Get(int id)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.Assessments.Find(id);
    }

    public int Delete(Assessment assessment)
    {
        var srDbContext = DALManager.GetContext();
        var result= srDbContext.Assessments.Remove(assessment);
        srDbContext.SaveChanges();
        if(result.State == EntityState.Detached)
            return 0;
        return -1;
    }
    
    public Tuple<int,Assessment?> Create(Assessment assessment)
    {
        var srDbContext = DALManager.GetContext();
        try
        {
            var ass = srDbContext.Assessments.Add(assessment);
            srDbContext.SaveChanges();
            if (ass.IsKeySet)
            {
                return new Tuple<int, Assessment>(0, ass.Entity);
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Error creating assessment: {0}", ex.Message);
            return new Tuple<int, Assessment>(1, null);
        }
        Logger.Error("Unkown error creating assessment");
        return new Tuple<int, Assessment?>(-1, null);
    }
    
    public List<AssessmentAnswer>? GetAnswers(int id)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.AssessmentAnswers.Where(a => a.AssessmentId == id).ToList();
    }
    
    public AssessmentAnswer? GetAnswer(int assessmentId, int questionId, string answerText)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.AssessmentAnswers.FirstOrDefault(a => a.AssessmentId == assessmentId
                                                                 && a.QuestionId == questionId
                                                                 && a.Answer == answerText);
    }

    public List<AssessmentQuestion>? GetQuestions(int id)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.AssessmentQuestions.Where(a => a.AssessmentId == id).ToList();
    }

    public AssessmentQuestion? GetQuestion(int id, string question)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.AssessmentQuestions.FirstOrDefault(a => a.AssessmentId == id && a.Question == question);
    }
    
    public AssessmentQuestion? GetQuestionById(int id, int questionId)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.AssessmentQuestions.FirstOrDefault(a => a.AssessmentId == id && a.Id == questionId);
    }
    
    public AssessmentQuestion? SaveQuestion(AssessmentQuestion question)
    {
        var srDbContext = DALManager.GetContext();
        if (srDbContext.Assessments.FirstOrDefault(ass => ass.Id == question.AssessmentId) is null)
        {
            throw new InvalidReferenceException($"The assessment {question.AssessmentId} indicated on the question does not exists");
        }
        srDbContext.AssessmentQuestions.Add(question);
        srDbContext.SaveChanges();
        return question;
    }
    
    public AssessmentAnswer? SaveAnswer(AssessmentAnswer answer)
    {
        var srDbContext = DALManager.GetContext();
        if (srDbContext.Assessments.FirstOrDefault(ass => ass.Id == answer.AssessmentId) is null)
        {
            throw new InvalidReferenceException($"The assessment {answer.AssessmentId} indicated on the answer does not exists");
        }
        if (srDbContext.AssessmentQuestions.FirstOrDefault(ass => ass.Id == answer.QuestionId) is null)
        {
            throw new InvalidReferenceException($"The question {answer.QuestionId} indicated on the answer does not exists");
        }
        srDbContext.AssessmentAnswers.Add(answer);
        srDbContext.SaveChanges();
        return answer;
    }
    
}