namespace Extentions
{
    public enum Owner
    {
        Player, // Controlled by player directly
        Ally, // Allied to player but not controlled by them
        Neutral, // Nor hostile nor friendly to anyone, not controlled by anybody
        Enemy // Hostile to player and ally, controlled by AI
    }
}