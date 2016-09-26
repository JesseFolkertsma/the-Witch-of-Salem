using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListSorting : MonoBehaviour {

    public List<int> list = new List<int>();

    void Start()
    {
        list.Add(3);
        list.Add(6);
        list.Add(1);
        list.Add(2);
        list.Add(0);
        list.Add(6);
        list.Add(5);
        list.Add(5);
        list.Add(5);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //List<int> tSort = SortList(list);
            //list.Clear();
            //for(int i = 0; i<tSort.Count; i++)
            //{
            //    list.Add(tSort[i]);
            //}
            
            print(GetHigh(list));
        }
    }
	
    List<int> SortList(List<int> lijsie)
    {
        List<int> sortedList = new List<int>();

        bool sorted = false;
        int count = 0;

        while(sorted == false){
            for (int i = 0; i < lijsie.Count; i++)
            {
                if (lijsie[i] == count)
                {
                    sortedList.Add(lijsie[i]);
                }
                if(sortedList.Count == lijsie.Count)
                {
                    sorted = true;
                }
            }
            count++;
        }

        return sortedList;
    }

    int GetHigh(List<int> l)
    {
        l.Sort();

        int counter = 0;
        int cHigh = 0;

        for (int i = 0; i < l.Count; i++)
        {
            bool b = false;
            for(int i2 = 0; b == false; i2++)
            {
                int tCounter = 0;
                if(l[i + i2] == i)
                {
                    tCounter++;
                    if(tCounter >= counter)
                    {
                        counter = tCounter;
                        cHigh = i;
                    }
                }
                else
                {
                    b = true;
                }
            }
        }

        return cHigh;
    }
}
