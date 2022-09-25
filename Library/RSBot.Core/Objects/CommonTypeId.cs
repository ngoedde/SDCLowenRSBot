namespace RSBot.Core.Objects;

public static class CommonTypeId
{
    //Potions
    public static TypeIdFilter HealthRecoveryPotion = new(3, 3, 1, 1);
    public static TypeIdFilter ManaRecoveryPotion = new(3, 3, 1, 2);
    public static TypeIdFilter VigorRecoveryPotion = new(3, 3, 1, 4);

    //Recovery (bad status)
    public static TypeIdFilter UniversalPill = new(3, 3, 2, 6);
    public static TypeIdFilter PurificationPill = new(3, 3, 2, 7);
    public static TypeIdFilter AbnormalStatePotion = new(3, 3, 2, 9);

    //Pet recovery
    public static TypeIdFilter PetBadStatusPotion = new(3, 3, 2, 7);

    //Summon pets
    public static TypeIdFilter GrowthPet = new(3, 2, 1, 1);
    public static TypeIdFilter GrabPet = new(3, 2, 1, 2);
    public static TypeIdFilter FellowPet = new(3, 2, 1, 3);
    
}