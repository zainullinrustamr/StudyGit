using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.Solution.Project;

namespace DirRX.Solution
{
  partial class ProjectTeamMembersSharedHandlers
  {

    public override void TeamMembersGroupChanged(Sungero.Domain.Shared.EnumerationPropertyChangedEventArgs e)
    {
      if (e.OldValue != e.OriginalValue)
      {  
        base.TeamMembersGroupChanged(e);
      }
    }
  }

  partial class ProjectSharedHandlers
  {

  }
}