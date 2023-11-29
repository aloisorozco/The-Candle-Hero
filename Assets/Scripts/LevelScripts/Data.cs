

[System.Serializable]
public class Data
{
    public int embers;
    public bool dashUpgrade;
    public bool doubleJumpUpgrade;
    public bool wallJumpUpgrade;

    public string currentScene;
    public string respawnPoint;
    public int lives;
    public float lightRadius;

    public string dataFile;

    public Data()
    {
        this.embers = 0;
        this.dashUpgrade = false;
        this.doubleJumpUpgrade = false;
        this.wallJumpUpgrade = false;
        this.currentScene = "Tutorial";
        this.respawnPoint = "InitialRespawnPoint";
        this.lives = 3;
        this.lightRadius = 5;
    }
    public Data(int embers, bool dashUpgrade, bool doubleJumpUpgrade, bool wallJumpUpgrade, string currentScene, string respawnPoint, int lives, float lightRadius, string dataFile)
    {
        this.embers = embers;
        this.dashUpgrade = dashUpgrade;
        this.doubleJumpUpgrade = doubleJumpUpgrade;
        this.wallJumpUpgrade = wallJumpUpgrade;
        this.currentScene = currentScene;
        this.respawnPoint = respawnPoint;
        this.lives = lives;
        this.lightRadius = lightRadius;
        this.dataFile = dataFile;
    }
}
