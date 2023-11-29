

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

    public bool[] emberTutorial;
    public bool[] emberLevel1;
    public bool[] emberLevel2;
    public bool[] emberLevel3;
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
        this.emberTutorial = new bool[] {false, false, false };
        this.emberLevel1 = new bool[] { false, false, false, false, false, false };
        this.emberLevel2 = new bool[] { false, false, false, false, false};
        this.emberLevel3 = new bool[] { false, false, false, false, false };
    }
    public Data(int embers, bool dashUpgrade, bool doubleJumpUpgrade, bool wallJumpUpgrade, string currentScene, string respawnPoint, int lives, float lightRadius, string dataFile, bool[] emberTutorial, bool[] emberLevel1, bool[] emberLevel2, bool[] emberLevel3)
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
        this.emberTutorial = emberTutorial;
        this.emberLevel1 = emberLevel1;
        this.emberLevel2 = emberLevel2;
        this.emberLevel3 = emberLevel3;
    }
}
