namespace OnlineCoaching.Domain.Enums
{
    public enum ClientStatus
    {
        Pending,   // 🟥 لسه مسجل جديد – كل الخدمات مقفولة
        Active,    // 🟩 اتوافق عليه – يقدر يستخدم الخدمات
        Suspended  // ⛔️ اتوقف مؤقتاً
    }
}
