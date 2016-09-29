﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Students
{
    public partial class ViewPosition : BaseEntityPortalPage<InternshipPositionGroup>
    {
        protected override void Fill()
        {
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionGroupRepository(UnitOfWork).Load(positionID, x => x.PhysicalObjects, x => x.Academics);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            ucPositionGroupView.Entity = Entity;
            ucPositionGroupView.Bind();

            base.OnLoad(e);
        }
    }
}
