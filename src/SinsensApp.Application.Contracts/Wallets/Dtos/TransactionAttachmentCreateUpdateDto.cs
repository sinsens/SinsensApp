using System;

namespace SinsensApp.Wallets.Dtos
{
    [Serializable]
    public class TransactionAttachmentCreateUpdateDto
    {
        public string Title { get; set; }

        public long Size { get; set; }

        public int Index { get; set; }

        public string Url { get; set; }
    }
}