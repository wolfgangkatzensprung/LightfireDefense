using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemiesData
{
    internal enum EnemyType
    {
        White = 0,
        Grey = 1,
        Black = 2,
        Red = 3,
        Blue = 4,
        Yellow = 5,
        Green = 6,
        Air = 7,
        Earth = 8,
        Fire = 9,
        Water = 10,
    }

    internal static EnemyType[][] enemyWaveTypes =
    {
        //1
        new EnemyType[]
        {
            EnemyType.White,
            EnemyType.Grey,
            EnemyType.Red,
            EnemyType.Blue,
            EnemyType.Yellow,
            EnemyType.Green,
        },
        //2
        new EnemyType[]
        {
            EnemyType.White,
            EnemyType.Grey,
            EnemyType.Red,
            EnemyType.Blue,
            EnemyType.Yellow,
            EnemyType.Green,
        },
        //3
        new EnemyType[]
        {
            EnemyType.White,
            EnemyType.Grey,
            EnemyType.Red,
            EnemyType.Blue,
            EnemyType.Yellow,
            EnemyType.Green,
        },
        //4
        new EnemyType[]
        {
            EnemyType.Black,
            EnemyType.Grey,
            EnemyType.Red,
            EnemyType.Blue,
            EnemyType.Yellow,
            EnemyType.Green,
        },
        //5
        new EnemyType[]
        {
            EnemyType.Black,
            EnemyType.Grey,
            EnemyType.Red,
            EnemyType.Blue,
            EnemyType.Yellow,
            EnemyType.Green,
        },
        //6
        new EnemyType[]
        {
            EnemyType.White,
            EnemyType.Grey,
            EnemyType.Black,
            EnemyType.Red,
            EnemyType.Blue
        },
        //7
        new EnemyType[]
        {
            EnemyType.White,
            EnemyType.Grey,
            EnemyType.Black,
            EnemyType.Green,
            EnemyType.Yellow
        },
        //8
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.Black,
            EnemyType.White
        },
        //9
        new EnemyType[]
        {
            EnemyType.Black,
            EnemyType.White
        },
        //10
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.Air,
        },
        //11
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.Earth
        },
        //12
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.Fire
        },
        //13
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.Water
        },
        //14
        new EnemyType[]
        {
            EnemyType.White,
        },
        //15
        new EnemyType[]
        {
            EnemyType.Blue,
            EnemyType.Water
        },
        //16
        new EnemyType[]
        {
            EnemyType.Blue,
            EnemyType.Water
        },
        //17
        new EnemyType[]
        {
            EnemyType.Blue,
            EnemyType.Water
        },
        //18
        new EnemyType[]
        {
            EnemyType.Red,
            EnemyType.Fire
        },
        //19
        new EnemyType[]
        {
            EnemyType.Red,
            EnemyType.Fire
        },
        //20
        new EnemyType[]
        {
            EnemyType.Red,
            EnemyType.Fire
        },
        //21
        new EnemyType[]
        {
            EnemyType.Yellow,
            EnemyType.Air
        },
        //22
        new EnemyType[]
        {
            EnemyType.Yellow,
            EnemyType.Air
        },
        //23
        new EnemyType[]
        {
            EnemyType.Yellow,
            EnemyType.Air
        },
        //24
        new EnemyType[]
        {
            EnemyType.Green,
            EnemyType.Earth
        },
        //25
        new EnemyType[]
        {
            EnemyType.Green,
            EnemyType.Earth
        },
        //26
        new EnemyType[]
        {
            EnemyType.Green,
            EnemyType.Earth
        },
        //27
        new EnemyType[]
        {
            EnemyType.Yellow,
            EnemyType.Blue
        },
        //28
        new EnemyType[]
        {
            EnemyType.Yellow,
            EnemyType.Red
        },
        //29
        new EnemyType[]
        {
            EnemyType.Yellow,
            EnemyType.Green
        },
        //30
        new EnemyType[]
        {
            EnemyType.Grey,
        },
        //31
        new EnemyType[]
        {
            EnemyType.Blue,
            EnemyType.Green
        },
        //32
        new EnemyType[]
        {
            EnemyType.Red,
            EnemyType.Green
        },
        // 33
        new EnemyType[]
        {
            EnemyType.Blue,
            EnemyType.Red
        },
        //34
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
        },
        //35
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
        },
        //36
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
        },
        //37
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
        },
        //38
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
        },
        //39
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
        },
        //40
        new EnemyType[]
        {
            EnemyType.Grey,
        },
        //41
        new EnemyType[]
        {
            EnemyType.Black,
        },
        //42
        new EnemyType[]
        {
            EnemyType.White,
        },
        //43
        new EnemyType[]
        {
            EnemyType.Red,
        },
        //44
        new EnemyType[]
        {
            EnemyType.Blue,
        },
        //45
        new EnemyType[]
        {
            EnemyType.Green,
        },
        //46
        new EnemyType[]
        {
            EnemyType.Yellow,
        },
        //47
        new EnemyType[]
        {
            EnemyType.Blue,
            EnemyType.Red,
            EnemyType.Yellow,
            EnemyType.Green
        },
        //48
        new EnemyType[]
        {
            EnemyType.Blue,
            EnemyType.Red,
            EnemyType.Yellow,
            EnemyType.Green
        },
        //49
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
            EnemyType.Blue,
            EnemyType.Red,
            EnemyType.Yellow,
            EnemyType.Green,
            EnemyType.Water,
            EnemyType.Fire,
            EnemyType.Air,
            EnemyType.Earth,
        },
        //50
        new EnemyType[]
        {
            EnemyType.Grey,
            EnemyType.White,
            EnemyType.Black,
            EnemyType.Blue,
            EnemyType.Red,
            EnemyType.Yellow,
            EnemyType.Green
        },
    };

    internal static int[] enemiesPerWave =
        { 8, 11, 13, 17, 19,
        23, 26, 29, 32, 35,
        38, 41, 44, 47, 50,
        50, 50, 50, 50, 55,
        60, 60, 60, 60, 65,
        70, 70, 70, 70, 75,
        80, 80, 80, 80, 85,
        90, 90, 90, 90, 95,
        100, 110, 120, 130, 140,
        150, 200, 300, 400, 500
    };

}
