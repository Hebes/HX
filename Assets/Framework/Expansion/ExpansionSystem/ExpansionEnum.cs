namespace Core
{
    public static class ExpansionEnum
    {
        public static int ToInt(this System.Enum e) => e.GetHashCode();
    }
}
