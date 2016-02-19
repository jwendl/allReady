﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.Entity;

namespace AllReady.Models
{
    public partial class AllReadyDataAccessEF7 : IAllReadyDataAccess
    {
        IEnumerable<Skill> IAllReadyDataAccess.Skills
        {
            get
            {
                return _dbContext.Skills
                    .Include(s => s.ParentSkill)
                    .Include(s => s.OwningOrganization)
                    .ToList();
            }
        }

        Skill IAllReadyDataAccess.GetSkill(int skillId)
        {
            return _dbContext.Skills.ToList().SingleOrDefault(x => x.Id == skillId);
        }

        Task IAllReadyDataAccess.AddSkill(Skill value)
        {
            _dbContext.Skills.Add(value);
            return _dbContext.SaveChangesAsync();
        }

        Task IAllReadyDataAccess.DeleteSkill(int skillId)
        {
            var toDelete = _dbContext.Skills.Where(c => c.Id == skillId).SingleOrDefault();

            if (toDelete != null)
            {
                //Orphan child skills before deleting
                _dbContext.Skills
                    .Where(s => s.ParentSkillId == toDelete.Id)
                    .ForEachAsync(s => { s.ParentSkill = null; s.ParentSkillId = null; }).Wait();
                _dbContext.Skills.Remove(toDelete);
                return _dbContext.SaveChangesAsync();
            }
            return null;
        }

        Task IAllReadyDataAccess.UpdateSkill(Skill value)
        {
            _dbContext.Skills.Update(value);
            return _dbContext.SaveChangesAsync();
        }
    }
}
