using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.Solution.Project;

namespace DirRX.Solution.Server
{
  partial class ProjectFunctions
  {
    /// <summary>
    /// Запустить задачу о добавлении пользователя в проект
    /// </summary>
    /// <returns>Уведомление</returns>
    public void CreateNotificationInsertProjectMember()
    {
      var addedParticipant = _obj.State.Properties.TeamMembers.Added
        .Where(t => t.State.IsInserted).ToList();
      foreach (var addedUser in addedParticipant)
      {
        var task = Sungero.Workflow.SimpleTasks.Create();
        var step1 = task.RouteSteps.AddNew();
        step1.AssignmentType = Sungero.Workflow.SimpleTask.AssignmentType.Notice;
        step1.Performer = addedUser.Member;
        task.Subject = string.Format(Projects.Resources.TaskInsertSubjectTemplate, _obj.Name, _obj.Info.Properties.TeamMembers.Properties.Group.GetLocalizedValue(addedUser.Group));
        task.Start();
      }      
    }
    
    ///
    ///<summary>
    /// Запустить задачу о изменении пользователя в проект
    /// </summary>
    /// <returns>Уведомление</returns>
    public void CreateNotificationChangedProjectMember()
    {
      var changedParticipant = _obj.State.Properties.TeamMembers.Changed
        .Where(t => !t.State.IsInserted).ToList();
      foreach (var User in changedParticipant)
      {
        if (User.Member != User.State.Properties.Member.OriginalValue)
        {
          //Добавлении в проект
          var task = Sungero.Workflow.SimpleTasks.Create();
          var step1 = task.RouteSteps.AddNew();
          step1.AssignmentType = Sungero.Workflow.SimpleTask.AssignmentType.Notice;
          step1.Performer = User.Member;
          task.Subject = string.Format(Projects.Resources.TaskInsertSubjectTemplate, _obj.Name, _obj.Info.Properties.TeamMembers.Properties.Group.GetLocalizedValue(User.Group));
          task.Start();
          
          //Удалении из проекта
          var deleteTask = Sungero.Workflow.SimpleTasks.Create();
          var deletestep1 = deleteTask.RouteSteps.AddNew();
          deletestep1.AssignmentType = Sungero.Workflow.SimpleTask.AssignmentType.Notice;
          deletestep1.Performer = User.State.Properties.Member.OriginalValue;
          deleteTask.Subject = string.Format(Projects.Resources.TaskChangedSubjectTemplate, _obj.Name, _obj.Info.Properties.TeamMembers.Properties.Group.GetLocalizedValue(User.Group));
          deleteTask.Start();
        }
        else
        {
          if (User.State.Properties.Group.OriginalValue != User.Group)
          {
            //Изменении роли
            var deleteTask = Sungero.Workflow.SimpleTasks.Create();
            var deletestep1 = deleteTask.RouteSteps.AddNew();
            deletestep1.AssignmentType = Sungero.Workflow.SimpleTask.AssignmentType.Notice;
            deletestep1.Performer = User.Member;
            deleteTask.Subject = string.Format(Projects.Resources.TaskChangedRoleSubjectTemplate, _obj.Info.Properties.TeamMembers.Properties.Group.GetLocalizedValue(User.State.Properties.Group.OriginalValue), _obj.Info.Properties.TeamMembers.Properties.Group.GetLocalizedValue(User.Group));
            deleteTask.Start();
          }
        }
      }     
    }
    
    ///
    ///<summary>
    /// Запустить задачу о удалении пользователя из проект
    /// </summary>
    /// <returns>Уведомление</returns>
    public void CreateNotificationDeletedProjectMember()    
    {
      var deletedParticipant = _obj.State.Properties.TeamMembers.Deleted;
      foreach (var User in deletedParticipant)
      {
        var task = Sungero.Workflow.SimpleTasks.Create();
        var step1 = task.RouteSteps.AddNew();
        step1.AssignmentType = Sungero.Workflow.SimpleTask.AssignmentType.Notice;
        step1.Performer = User.Member;
        task.Subject = string.Format(Projects.Resources.TaskDeletedSubjectTemplate, _obj.Name, _obj.Info.Properties.TeamMembers.Properties.Group.GetLocalizedValue(User.Group));
        task.Start();
      }
    }
  }
}