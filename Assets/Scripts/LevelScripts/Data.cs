

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
    public int maxLives;
    public float lightRadius;

    public string dataFile;

    public bool[] emberTutorial;
    public bool[] emberLevel1;
    public bool[] emberLevel2;
    public bool[] emberLevel3;

    public bool[] activeLevels;

    public bool dashPlus;
    public bool jumpPlus;
    public bool doubleJumpPlus;
    public float healthRate;
    public Data()
    {
        this.embers = 0;
        this.dashUpgrade = false;
        this.doubleJumpUpgrade = false;
        this.wallJumpUpgrade = false;
        this.currentScene = "Tutorial";
        this.respawnPoint = "InitialRespawnPoint";
        this.lives = 5;
        this.maxLives =5;
        this.lightRadius = 5;
        this.emberTutorial = new bool[] { false, false };
        this.emberLevel1 = new bool[] { false, false, false, false, false, false };
        this.emberLevel2 = new bool[] { false, false, false, false, false};
        this.emberLevel3 = new bool[] { false, false, false, false, false };
        this.activeLevels = new bool[] { true, false, false };
        this.dashPlus = false;
        this.jumpPlus = false;
        this.doubleJumpPlus = false;
        this.healthRate = 1;

        
    }
    public Data(int embers, bool dashUpgrade, bool doubleJumpUpgrade, 
        bool wallJumpUpgrade, string currentScene, string respawnPoint, 
        int lives, int maxLives, float lightRadius, string dataFile, 
        bool[] emberTutorial, bool[] emberLevel1, bool[] emberLevel2, 
        bool[] emberLevel3, bool[] activeLevels, bool dashPlus, 
        bool jumpPlus, bool doubleJumpPlus ,float healthRate)
    {
        this.embers = embers;
        this.dashUpgrade = dashUpgrade;
        this.doubleJumpUpgrade = doubleJumpUpgrade;
        this.wallJumpUpgrade = wallJumpUpgrade;
        this.currentScene = currentScene;
        this.respawnPoint = respawnPoint;
        this.lives = lives;
        this.maxLives = maxLives;
        this.lightRadius = lightRadius;
        this.dataFile = dataFile;
        this.emberTutorial = emberTutorial;
        this.emberLevel1 = emberLevel1;
        this.emberLevel2 = emberLevel2;
        this.emberLevel3 = emberLevel3;
        this.activeLevels = activeLevels;
        this.dashPlus = dashPlus;
        this.jumpPlus = jumpPlus;
        this.doubleJumpPlus = doubleJumpPlus;
        this.healthRate = healthRate;
    }
}
