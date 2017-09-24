using System;
using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer
{
    public enum ApprovalStatusEntity 
    {
        Disapproved = 0,
        Approved = 1
    }

    public static class ApprovalStatusEntityExtensions 
    {
        public static ApprovalStatusEntity ToEntityModel(this ApprovalStatus approvalStatus) 
        {
            switch (approvalStatus)
            {
                case ApprovalStatus.Disapproved:
                    return ApprovalStatusEntity.Disapproved;

                case ApprovalStatus.Approved:
                    return ApprovalStatusEntity.Approved;

                default:
                    throw new NotSupportedException();
            }
        }

        public static ApprovalStatus ToDomainModel(this ApprovalStatusEntity approvalStatusEntity) 
        {
            switch (approvalStatusEntity)
            {
                case ApprovalStatusEntity.Disapproved:
                    return ApprovalStatus.Disapproved;

                case ApprovalStatusEntity.Approved:
                    return ApprovalStatus.Approved;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
