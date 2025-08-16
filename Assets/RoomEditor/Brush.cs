public abstract class Brush
{
    public string Name { get; }
    public string Description { get; }
    public Editor.BrushType BrushType { get; }

    protected Brush(string name, string description, Editor.BrushType brushType)
    {
        Name = name;
        Description = description;
        BrushType = brushType;
    }

    public abstract void Paint(Editor_Tile[,] grid, int startX, int startY);
}

public class Brush_none : Brush
{
    public Brush_none() : base("None", "None\nBasically an eraser", Editor.BrushType.None) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.None);
    }
}
public class Brush_obstacleNoShoot1x1 : Brush
{
    public Brush_obstacleNoShoot1x1() : base("1x1 Obstacle", "1x1 Obstacle\nNo one can shoot over it", Editor.BrushType.Obstacle_noShoot_1x1) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_1x1);
    }
}
public class Brush_obstacleNoShoot2x1 : Brush
{
    public Brush_obstacleNoShoot2x1() : base("2x1 Obstacle", "2x1 Obstacle\nNo one can shoot over it", Editor.BrushType.Obstacle_noShoot_2x1) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        try
        {
            grid[startX, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_2x1);
            grid[startX + 1, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_2x1);
        }
        catch { }
    }
}
public class Brush_obstacleNoShoot3x1 : Brush
{
    public Brush_obstacleNoShoot3x1() : base("3x1 Obstacle", "3x1 Obstacle\nNo one can shoot over it", Editor.BrushType.Obstacle_noShoot_3x1) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        try
        {
            grid[startX, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_3x1);
            grid[startX + 1, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_3x1);
            grid[startX + 2, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_3x1);
        }
        catch { }
    }
}
public class Brush_obstacleNoShoot2x2 : Brush
{
    public Brush_obstacleNoShoot2x2() : base("2x2 Obstacle", "2x2 Obstacle\nNo one can shoot over it", Editor.BrushType.Obstacle_noShoot_2x2) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        try
        {
            grid[startX, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_2x2);
            grid[startX + 1, startY].SetVisual(Editor.BrushType.Obstacle_noShoot_2x2);
            grid[startX, startY + 1].SetVisual(Editor.BrushType.Obstacle_noShoot_2x2);
            grid[startX + 1, startY + 1].SetVisual(Editor.BrushType.Obstacle_noShoot_2x2);
        }
        catch { }
    }
}
public class Brush_obstacleShoot1x1 : Brush
{
    public Brush_obstacleShoot1x1() : base("1x1 Obstacle", "1x1 Obstacle\nEveryone can shoot over it", Editor.BrushType.Obstacle_shoot_1x1) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Obstacle_shoot_1x1);
    }
}
public class Brush_lightsource : Brush
{
    public Brush_lightsource() : base("Lightsource", "Lightsource", Editor.BrushType.Lightsource) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Lightsource);
    }
}
public class Brush_lootbox : Brush
{
    public Brush_lootbox() : base("Lootbox", "Lootbox\nChest, bag, just some loot man idk.", Editor.BrushType.Lootbox) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Lootbox);
    }
}
public class Brush_resource : Brush
{
    public Brush_resource() : base("Resource", "Resource\nMinable or gatherable resource like mineral, stone, bush etc.", Editor.BrushType.Resource) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Resource);
    }
}
public class Brush_enemyMeleeAggresive : Brush
{
    public Brush_enemyMeleeAggresive() : base("Enemy melee aggressive", "Enemy melee aggressive\nThis bad boi will rush player right when he enters the room", Editor.BrushType.Enemy_mAggresive) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Enemy_mAggresive);
    }
}
public class Brush_enemyMeleeEvasive : Brush
{
    public Brush_enemyMeleeEvasive() : base("Enemy melee evasive", "Enemy melee evasive\nRushes player while dodging from side to side.", Editor.BrushType.Enemy_mEvasive) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Enemy_mEvasive);
    }
}
public class Brush_enemyMeleeWandering : Brush
{
    public Brush_enemyMeleeWandering() : base("Enemy melee wandering", "Enemy melee wandering\nWanders randomly, attacks player when close.", Editor.BrushType.Enemy_mWandering) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Enemy_mWandering);
    }
}
public class Brush_enemyMeleeStealth : Brush
{
    public Brush_enemyMeleeStealth() : base("Enemy melee stealth", "Enemy melee stealth\nSpawns and attacks player when close.", Editor.BrushType.Enemy_mStealth) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Enemy_mStealth);
    }
}
public class Brush_enemyRangedStatic : Brush
{
    public Brush_enemyRangedStatic() : base("Enemy ranged static", "Enemy ranged static\nShoots player when close. Mostly turrets.", Editor.BrushType.Enemy_rStatic) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Enemy_rStatic);
    }
}
public class Brush_enemyRangedWandering : Brush
{
    public Brush_enemyRangedWandering() : base("Enemy ranged wandering", "Enemy ranged wandering\nWanders and shoots while keeping distance.", Editor.BrushType.Enemy_rWandering) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Enemy_rWandering);
    }
}
public class Brush_trap : Brush
{
    public Brush_trap() : base("Trap", "Trap\nPlaceable source of various types of damage.", Editor.BrushType.Trap) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Trap);
    }
}
public class Brush_specificObject : Brush
{
    public Brush_specificObject() : base("Specific object", "Specific object\nChoose any object you would like to place.", Editor.BrushType.Specific) { }

    public override void Paint(Editor_Tile[,] grid, int startX, int startY)
    {
        grid[startX, startY].SetVisual(Editor.BrushType.Specific);
    }
}
