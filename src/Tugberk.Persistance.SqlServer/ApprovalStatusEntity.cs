using System;
using Tugberk.Domain;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Persistance.SqlServer
{
    public enum ApprovalStatusEntity 
    {
        Disapproved = 0,
        Approved = 1
    }

    public static class ApprovalStatusEntityExtensions 
    {
        public static ApprovalStatusEntity ToEntityModel(this ApprovalStatusReadModel approvalStatus) 
        {
            switch (approvalStatus)
            {
                case ApprovalStatusReadModel.Disapproved:
                    return ApprovalStatusEntity.Disapproved;

                case ApprovalStatusReadModel.Approved:
                    return ApprovalStatusEntity.Approved;

                default:
                    throw new NotSupportedException();
            }
        }

        public static ApprovalStatusReadModel ToDomainModel(this ApprovalStatusEntity approvalStatusEntity) 
        {
            switch (approvalStatusEntity)
            {
                case ApprovalStatusEntity.Disapproved:
                    return ApprovalStatusReadModel.Disapproved;

                case ApprovalStatusEntity.Approved:
                    return ApprovalStatusReadModel.Approved;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
