using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;

public class DBManager : MonoBehaviour
{

    public static DBManager instance = null;

    public string dbName = "MikoDB.db";
    private SqliteConnection dbConnector = null;
    private SqliteCommand dbCommand = null;
    private SqliteDataReader dbReader = null;

    private  string dialogTable = "Dialogue";
    private  string opTable = "OptionInfo";
    private List<string> tmpValuesList = new List<string>();


    private void Awake()
    {
        instance = this;   
    }

  
    public void InitDB()
    {
        OpenDB();
    }


    public void OpenDB()
    {
        try
        {
            string Dbpath = Application.dataPath;

            string conn = "data source=" + Dbpath + "/" + dbName;
            dbConnector = new SqliteConnection(conn);
            dbConnector.Open();
            EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Bug, 1, "DBSuccess!!!");
            

            var res = TableExists(dialogTable);
            if (res.Read())
            {
                int c = res.GetInt32(0);
                if (c == 0)
                    CreateDialogueInfoTable();
            }

            res = TableExists(opTable);
            if (res.Read())
            {
                int c = res.GetInt32(0);
                if (c == 0)
                    CreateOptionInfo();
            }

        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            EventManager.instance.SendEvent((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.Bug,  1, e.ToString());

            string path = Application.dataPath;
            path += "/" + "123123.txt";

            if (!File.Exists(path))
                File.Create(path).Dispose();
            File.WriteAllText(path, e.ToString()+ "    " + e.StackTrace);

        }

    }


    public SqliteDataReader ExecuteQuery(string sqlQuery)

    {

        dbCommand = dbConnector.CreateCommand();

        dbCommand.CommandText = sqlQuery;
        
        dbReader = dbCommand.ExecuteReader();

        return dbReader;

    }


    public SqliteDataReader TableExists(string name)
    {
        string query = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' and name='"+name+ "'";
        return ExecuteQuery(query);
    }

    public SqliteDataReader CreateTable(string name, string[] type, string[] rowName)
    {
        string query = "CREATE TABLE " + name + "(" + rowName[0] + " " + type[0];
        for (int i=1; i< type.Length; ++i)
        {
            query += ", " + rowName[i] + " " + type[i];
        }
        query += ")";

        return ExecuteQuery(query);
    }
    

    public void CreateDialogueInfoTable()
    {
        string[] rowName = new string[] {
            "id",
            "type",
            "love",
            "canRandom",
            "isChatBubble",
            "content",
            "optionIds",
            "voice",
            "animation",
        };
        string[] type = new
            string[] {
                "INT" ,
                "INT" ,
                "INT" ,
                "INT" ,
                "INT" ,
                "VARCHAR(128)",
                "VARCHAR(128)",
                "VARCHAR(128)",
                "VARCHAR(128)",
              };

        var d = CreateTable(dialogTable, type, rowName);
        



        if (d != null)
        {

        }



    }

    public void CreateOptionInfo()
    {
        string[] rowName = new string[] {
            "id",
            "DialogueId",
            "addLove",
            "content",
        };
        string[] type = new
            string[] {
                "INT" ,
                "INT" ,
                "INT" ,
                "VARCHAR(128)",
              };

        var d = CreateTable(opTable, type, rowName);


        OptionInfo w = new OptionInfo();
        w.Id = 2;
        w.DialogueId = 3;
        w.addLove = 0;
        w.content = "qwe";
        var list = GetOptionInfoList(w);
        InsertToTable(opTable, list);
    }


    public void UpdateTable()
    {

    }

    public SqliteDataReader InsertToTable(string name, List<string> values)
    {
        string query = "INSERT INTO " + name + " VALUES (" + "'" + values[0]+"'";

        for (int i = 1; i < values.Count; ++i)
        {

            query += ", '" + values[i] + "'";

        }

        query += ")";
        return ExecuteQuery(query);
    }


    public List<string> GetDialogueList(DialogueInfo info)
    {
        tmpValuesList.Clear();
        tmpValuesList.Add(info.Id.ToString());
        tmpValuesList.Add(info.type.ToString());
        tmpValuesList.Add(info.love.ToString());
        tmpValuesList.Add(info.canRandom.ToString());
        tmpValuesList.Add(info.isChatBubble.ToString());
        tmpValuesList.Add(info.content);
        tmpValuesList.Add(OptionIdToString(info.optionIds));
        tmpValuesList.Add(info.voice);
        tmpValuesList.Add(info.animation);
        return tmpValuesList;
    }

    public List<string> GetOptionInfoList(OptionInfo info)
    {
        tmpValuesList.Clear();
        tmpValuesList.Add(info.Id.ToString());
        tmpValuesList.Add(info.DialogueId.ToString());
        tmpValuesList.Add(info.addLove.ToString());
        tmpValuesList.Add(info.content);
        return tmpValuesList;
    }

    public string OptionIdToString(List<int> list)
    {
        if (list.Count == 0)
            return string.Empty;
        string s = list[0].ToString();
        for (int i =1; i< list.Count; ++i)
        {
            s += "|" + list[i].ToString();
        }

        return s;
    }

    public List<int> GetOptiuonIdFromString(string ids)
    {
        List<int> l = new List<int>();
        var ss = ids.Split('|');
        for (int i =0; i< ss.Length; ++i)
        {
            l.Add(int.Parse(ss[i]));
        }

        return l;
    }


    public void CloseDB()
    {
       

        if (dbReader != null)
            dbReader.Dispose();
        if (dbCommand != null)
            dbCommand.Dispose();

        if (dbConnector != null)
        {
            dbConnector.Close();
            dbConnector.Dispose();
        }
    }
}
