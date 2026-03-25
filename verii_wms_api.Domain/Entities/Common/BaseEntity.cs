using System;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTimeProvider.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    
        // User Informations
        public long? CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }

        public long? UpdatedBy { get; set; }
        public User? UpdatedByUser { get; set; }

        public long? DeletedBy { get; set; }
        public User? DeletedByUser { get; set; }

        /// <summary>
        /// Varlığı yumuşak silinir olarak işaretler.
        /// </summary>
        /// <param name="deletedByUserId">Silme işlemini gerçekleştiren kullanıcı ID'si (opsiyonel).</param>
        public void MarkAsDeleted(long? deletedByUserId = null)
        {
            IsDeleted = true;
            DeletedDate = DateTimeProvider.Now;
            DeletedBy = deletedByUserId;
        }

        /// <summary>
        /// Varlık güncelleme bilgilerini ayarlar.
        /// </summary>
        /// <param name="updatedByUserId">Güncelleme işlemini gerçekleştiren kullanıcı ID'si (opsiyonel).</param>
        public void SetUpdatedInfo(long? updatedByUserId = null)
        {
            UpdatedDate = DateTimeProvider.Now;
            UpdatedBy = updatedByUserId;
        }

    }
}
