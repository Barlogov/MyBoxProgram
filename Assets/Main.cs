using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int howManyStripsWillBe; // Сколько всего будет балок
    public int howManyBlocksWillBe; // Сколько всего будет блоков
    
    public GameObject stripsCollection; // Родительский объект, для всех балок
    public GameObject stripPrefab; // Префаб пустой балки
    
    public GameObject blockPrefab; // Префаб пустой блока
    
    [Header("Set Dynamically")]
    public GameObject[] strips; // Масив всех балок ( которые будут созданы)
    public int numberOfStrips = 0; // Какая по счёту балка создаётся
    public int sortingStrip = 0; // Какая по счёту балка заполняется
    public GameObject[] doneStrips; // Заполненые балки
    public int numOfDoneStrips; // Номер заполненной балки
    
    
    public GameObject[] blocks; // Масив всех блоков ( которые будут созданы)
    public int numberOfBlocks = 0; // Какой по счёту блок создаётся
    public int[] sortingBlocks; // Блоки в сортировке
    

    private void Start()
    {
        strips = new GameObject[howManyStripsWillBe];
        blocks = new GameObject[howManyBlocksWillBe];
        doneStrips = new GameObject[howManyStripsWillBe];
        
        sortingBlocks = new int[howManyBlocksWillBe];
        
        CreateStrips(3f, 2);
        CreateStrips(2f, 1);
        
        
        CreateBlocks(2f, 1);
        CreateBlocks(1.5f, 2);
        CreateBlocks(0.5f, 6);
        
        DoneFor1Step();
        DoneFor2Steps();
        
        
        
        //SortManager(new int[]{}, 0);
    }

    void CreateStrips(float mLength, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            
            strips[numberOfStrips] = Instantiate<GameObject>(stripPrefab); // Создать балку и поместить её в общий массив
            strips[numberOfStrips].name = "(Strip " + numberOfStrips.ToString()+") L: "+ mLength.ToString(); // Дать имя, соответствующее номеру
            strips[numberOfStrips].transform.SetParent(stripsCollection.transform); // Поместить балку в колекцию(объект на сцене)
            
            strips[numberOfStrips].GetComponent<Strip>().maxLength = mLength; // Задать размер в скрипте
            strips[numberOfStrips].GetComponent<Strip>().leftLength = mLength; // Задать размер в скрипте
            strips[numberOfStrips].GetComponent<Strip>().fillLength = 0; // Задать размер в скрипте
            
            strips[numberOfStrips].transform.position = new Vector3(0, 1.2f+numberOfStrips*1.2f, 0);

            numberOfStrips++;
        }
    }

    void CreateBlocks(float mLength, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            
            blocks[numberOfBlocks] = Instantiate<GameObject>(blockPrefab); // Создать балку и поместить её в общий массив
            blocks[numberOfBlocks].name = "(Block " + numberOfBlocks.ToString() +") L: "+ mLength.ToString(); // Дать имя, соответствующее номеру
            blocks[numberOfBlocks].transform.localScale = new Vector3(mLength, 1 , 1);
            
            //blocks[numberOfBlocks].transform.position = new Vector3(0, numberOfBlocks*1.2f, 0);

            numberOfBlocks++;
        }
    }

    void DoneFor1Step()
    {
        for(int s=0; s<howManyStripsWillBe; s++)
        {
            if(strips[sortingStrip] == null) continue;
            
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i] == null || strips[sortingStrip] == null) continue;
                if (Math.Round(blocks[i].transform.localScale.x, 3) ==
                    Math.Round(strips[sortingStrip].GetComponent<Strip>().maxLength, 3))
                {
                    strips[sortingStrip].GetComponent<Strip>().leftLength = 0;

                    blocks[i].transform.SetParent(strips[sortingStrip].transform); // Засовываем блок в балку
                    blocks[i].transform.localPosition = new Vector3(0, 0, 0);

                    doneStrips[numOfDoneStrips] = strips[sortingStrip];
                    strips[sortingStrip] = null;

                    blocks[i] = null; // Убрали, чтобы больше не запихивать его никуда
                    numOfDoneStrips++;
                }
            }
            
            sortingStrip++;
        }

        sortingStrip = 0;

    }
    void DoneFor2Steps()
    {
        for(int s = 0; s<howManyStripsWillBe; s++)
        {
            if(strips[sortingStrip] == null) continue;
            
            for (int j = 0; j < blocks.Length; j++)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[j] == null || blocks[i] == null || j == i || strips[sortingStrip] == null) continue; // Перейти к след операции

                    if ((Math.Round(blocks[j].transform.localScale.x, 3) +
                         Math.Round(blocks[i].transform.localScale.x, 3)) ==
                        Math.Round(strips[sortingStrip].GetComponent<Strip>().maxLength, 3))
                    {
                        // Если максимальный блок + другой заполняют полностью => заполнить и перенести в нужные массивы
                        strips[sortingStrip].GetComponent<Strip>().leftLength = 0;

                        blocks[j].transform.SetParent(strips[sortingStrip].transform); // Засовываем блок в балку
                        blocks[i].transform.SetParent(strips[sortingStrip].transform); // Засовываем блок в балку
                        blocks[j].transform.localPosition = new Vector3(0, 0, 0);
                        blocks[i].transform.localPosition = new Vector3(blocks[j].transform.localScale.x * 1.1f, 0, 0);

                        doneStrips[numOfDoneStrips] = strips[sortingStrip];
                        strips[sortingStrip] = null;

                        blocks[j] = null; // Убрали, чтобы больше не запихивать его никуда
                        blocks[i] = null; // Убрали, чтобы больше не запихивать его никуда
                        numOfDoneStrips++;
                    }
                }

            }
            
            sortingStrip++;
        }
        
        sortingStrip = 0;
    }
    void SortManager(int[] used, int usedNow)
    {
        if (strips[sortingStrip].GetComponent<Strip>().leftLength < 0.01f) // Если место в балке кончалось - выйти
        {
            doneStrips[numOfDoneStrips] = strips[sortingStrip];
            numOfDoneStrips++;
            sortingStrip++;
            return;
        }

        int m = MaxBlock(); // Максимальный на данный момент блок
        sortingBlocks[usedNow + 1] = m;
        //GameObject m = blocks[mInt]; // Максимальный на данный момент блок
        
        if (strips[sortingStrip].GetComponent<Strip>().leftLength < blocks[m].transform.localScale.x)
        {
            // Если места меньше, чем максимальный блок
            // To do
            sortingStrip++; // Только сейчас
        }
        else // Если больше
        {
            blocks[m].transform.SetParent(strips[sortingStrip].transform); // Засовываем блок в балку
            strips[sortingStrip].GetComponent<Strip>().leftLength -= blocks[m].transform.localScale.x; // Уменьшаем leftLength на длину блока

            sortingBlocks[usedNow] = m; // Которые сейчас запихиваются
            blocks[m] = null; // Убрали, чтобы больше не запихивать его никуда

            SortManager(sortingBlocks, m);

        }
    }
    
    int MaxBlock()
    {
        GameObject max = blocks[0];
        int maxInt = 0;
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != null && blocks[i].transform.localScale.x > max.transform.localScale.x)
            {
                max = blocks[i];
                maxInt = i;
            }
            
        }

        return (maxInt);
    }
    
}
