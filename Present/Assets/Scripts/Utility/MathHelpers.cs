public static class MathHelpers
{
    /**
     * Normal c# mod gets weird with negatives
     * 1 % 5 = 1 and 6 % 5 = 1
     * -1 % 5 = -1
     * 
     * This mod keeps your wrapping positive
     * Mod(-1, 5) = 4
     **/
    public static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
