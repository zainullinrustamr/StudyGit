using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.Solution.Project;

namespace DirRX.Solution
{
  partial class ProjectServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      
      base.BeforeSave(e);
    }

    public override void Saving(Sungero.Domain.SavingEventArgs e)
    {
      Functions.Project.CreateNotificationInsertProjectMember(_obj);
      Functions.Project.CreateNotificationChangedProjectMember(_obj);
      Functions.Project.CreateNotificationDeletedProjectMember(_obj);              
      base.Saving(e);
    }
  }

}