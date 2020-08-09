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

    public GameObject minPrefab; // Самый маленький блок ( для Max)
    public GameObject maxPrefab; // Самый большой блок ( для Max)
    
    [Header("Set Dynamically")]
    public GameObject[] strips; // Масив всех балок ( которые будут созданы)
    public int numberOfStrips = 0; // Какая по счёту балка создаётся
    public int sortingStrip = 0; // Какая по счёту балка заполняется
    public GameObject[] doneStrips; // Заполненые балки
    public int numOfDoneStrips = 0; // Номер заполненной балки
    public GameObject[] emptyStrips; // Пустые балки
    public int numOfEmptyStrips = 0; // Номер пустой балки
    
    
    public GameObject[] blocks; // Масив всех блоков ( которые будут созданы)
    public int numberOfBlocks = 0; // Какой по счёту блок создаётся
    public GameObject[] sortingBlocks; // Блоки в сортировке
    //public int sortingBlock; // 
    public float sortingLengthBlocks;

    private void Start()
    {
        strips = new GameObject[howManyStripsWillBe];
        doneStrips = new GameObject[howManyStripsWillBe];
        emptyStrips = new GameObject[howManyStripsWillBe];
        
        blocks = new GameObject[howManyBlocksWillBe];
        sortingBlocks = new GameObject[howManyBlocksWillBe];
        /* Task #1
        CreateStrips(3f, 2);
        CreateStrips(2f, 1);
        
        CreateBlocks(2f, 1);
        CreateBlocks(1.5f, 2);
        CreateBlocks(0.5f, 6);
        // Complete #1
        */ 
        
        /* Task #2
        CreateStrips(4.5f, 1);
        
        CreateBlocks(2f, 2);
        CreateBlocks(1.5f,1);
        CreateBlocks(1f, 1);
        // Complete #2
        */ 
        
        /* Task #3
        CreateStrips(4.5f, 1);
        
        CreateBlocks(2f, 2);
        */
        
        // Task #4
        CreateStrips(7f, 1);
        CreateStrips(2f,2);
        
        CreateBlocks(2f, 5);
        
        SortMaxLength();
        
        //DoneFor1Step();
        //DoneFor2Steps();
        /*
        for(int i=0; i<strips.Length; i++)
        {
            if(CountNotNull(strips) == 0) return;
            SortManager();
        }
        */

        //Debug.Log(MaxBlock()+" <---> "+ MinBlock());
        
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
    
    /// <summary>
    /// 
    /// - sortingB место в масиве sortingBlocks в которое запишется новый блок
    /// 
    /// </summary>
    void SortManager(int sortingB = 0 )
    {
        
        if (sortingB == 0)
        {
            
            int m = MaxBlock();
            if(m == -1) return;
            sortingBlocks[0] = blocks[m]; // Отправляем самый большой блок в 0ую позицию сортировачного масива
            sortingLengthBlocks += blocks[m].transform.localScale.x; // Увеличиваем длину сортировочного масива

            /* Блок на подобии DoneFor1Step() 
            if (sortingLengthBlocks == Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
            {
                strips[sortingStrip].GetComponent<Strip>().leftLength = 0;
                blocks[m] = null;
                
                sortingBlocks[0].transform.SetParent(strips[sortingStrip].transform);
                sortingBlocks[0] = null;
                
                doneStrips[numOfDoneStrips] = strips[sortingStrip];
                strips[sortingStrip] = null;
                
                numOfDoneStrips++;
                sortingStrip++;
                
                return;
            }
            */
            
            blocks[m] = null;
            
            SortManager(1);
        }
        else
        {
            // Первый прогон с точным совпадением и исключением, что если больше, то ничего не делать (Равно)
            for (int i = 0; i < blocks.Length; i++)
            {
                if(blocks[i]== null) continue; // Если блока нет - продолжить
                if(strips[sortingStrip] == null) continue; // Если нет балки - продолжить
                
                
                if (sortingLengthBlocks + blocks[i].transform.localScale.x >
                    strips[sortingStrip].GetComponent<Strip>().leftLength)
                { // Если блок + масив больше, чем балка продолжить
                    continue;
                }
                
                
                if ((sortingLengthBlocks +
                     Math.Round(blocks[i].transform.localScale.x, 3)) ==
                    Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                { // Если длина масива + длина блока = длине балки => Заполнить
                    strips[sortingStrip].GetComponent<Strip>().leftLength = 0;

                    sortingBlocks[sortingB] = blocks[i]; // Засовываем блок в сортированный масив
                    sortingLengthBlocks += blocks[i].transform.localScale.x; // Увеличиваем длину сортированого масива
                    blocks[i] = null; // Убрали, чтобы больше не запихивать его никуда
                    
                    for (int b = 0; b < sortingBlocks.Length; b++) // Заполняем балку(с идеальным заполнением) блоками
                    {
                        if(sortingBlocks[b] == null) continue;
                        sortingBlocks[b].transform.SetParent(strips[sortingStrip].transform);
                        sortingBlocks[b].transform.localPosition = new Vector3(b*(sortingBlocks[b].transform.localScale.x+1),0,0);
                        sortingBlocks[b] = null;
                    }

                    doneStrips[numOfDoneStrips] = strips[sortingStrip];
                    strips[sortingStrip] = null;

                    
                    numOfDoneStrips++;
                    sortingStrip++;
                    sortingLengthBlocks = 0;
                    return;
                }

                /*
                
                */
            }
            
            // Второй прогон с максимальным заполнением сортированного масива (Меньше)
            // Мы поднимаемся по масиву от самых больших до маленьких
            for (int i = 0; i < blocks.Length; i++)
            {
                if(blocks[i]== null) continue; // Если блока нет - продолжить
                if(strips[sortingStrip] == null) continue; // Если нет балки - продолжить
                
                // Здесь мы с самым большим blocks[i] блоком и если он меньше, чем длина балки
                if ((sortingLengthBlocks +
                     Math.Round(blocks[i].transform.localScale.x, 3)) <
                    Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                { // То надо добавить его в сорртированный масив и вызвать следующую итерацию
                    
                    // Не уверен, что это нужно, надо посмотреть куда дальше пойдёт сигнал ----------------------------------------------------------
                    
                    sortingBlocks[sortingB] = blocks[i]; // Засовываем блок в сортированный масив
                    sortingLengthBlocks += blocks[i].transform.localScale.x; // Увеличиваем длину сортированого масива
                    blocks[i] = null; // Убрали, чтобы больше не запихивать его никуда
                    
                   SortManager(sortingB+1);
                    
                }
            }
            
            // Третий прогон (Больше)
            // Если прогнали все балки по максимуму и нет точного равенства и не хватает для полного заполнения балки
            // Просчёт с изменением последнего блока в сортированном масиве и предпологаемого блока
            // Надо вернуть блок в пул и попробовать заполнить с блоками размера поменьше
            
            // TODO Добавить блок if для проверки, что я в нужном месте
            if(CountNotNull(strips) == 0 ) return;
            if(sortingLengthBlocks == 0 ) return;
            
            // TODO Момент, когда блоков в пуле больше нет
            
            // Выкидываю последний (самый маленький) блок из сортированного масива в пул и уменьшаю длину масива
            blocks[MaxBlock() - 1] = sortingBlocks[sortingB - 1];
            sortingLengthBlocks -= sortingBlocks[sortingB - 1].transform.localScale.x;
            sortingBlocks[sortingB - 1] = null;
            
            
            for (int i = 0; i < blocks.Length; i++)
            {
                if(blocks[i]== null) continue; // Если блока нет - продолжить
                
                for (int j = 0; j < blocks.Length; j++)
                {
                    if(blocks[j]== null) continue; // Если блока нет - продолжить
                    
                    if ((sortingLengthBlocks +
                         Math.Round(blocks[i].transform.localScale.x, 3) + Math.Round(blocks[j].transform.localScale.x, 3)) ==
                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                    { // Если длина масива + длина 1 блока + длина 2 блока = длине балки => Заполнить
                        strips[sortingStrip].GetComponent<Strip>().leftLength = 0;

                        sortingBlocks[sortingB-1] = blocks[i]; // Засовываем блок в сортированный масив
                        sortingLengthBlocks += blocks[i].transform.localScale.x; // Увеличиваем длину сортированого масива
                        blocks[i] = null; // Убрали, чтобы больше не запихивать его никуда
                        
                        sortingBlocks[sortingB] = blocks[j]; // Засовываем блок в сортированный масив
                        sortingLengthBlocks += blocks[j].transform.localScale.x; // Увеличиваем длину сортированого масива
                        blocks[j] = null; // Убрали, чтобы больше не запихивать его никуда
                        
                        for (int b = 0; b < sortingBlocks.Length; b++) // Заполняем балку(с идеальным заполнением) блоками
                        {
                            if(sortingBlocks[b] == null) continue;
                            sortingBlocks[b].transform.SetParent(strips[sortingStrip].transform);
                            sortingBlocks[b].transform.localPosition = new Vector3(b*(sortingBlocks[b].transform.localScale.x+1),0,0);
                            sortingBlocks[b] = null;
                        }

                        doneStrips[numOfDoneStrips] = strips[sortingStrip];
                        strips[sortingStrip] = null;

                    
                        numOfDoneStrips++;
                        sortingStrip++;
                        sortingLengthBlocks = 0;
                        return;
                    }
                }
            }

                    
            

            //sortingLengthBlocks = 0;
            
        }
        
        
        
        
        
    }

    /// <summary>
    ///  Заполняет балку блоками, подходящими идиально по длине (макс 7 блоков)
    /// </summary>
    void SortMaxLength()
    {
        for (int a = 0; a < blocks.Length; a++)
        {
            for (int b = 0; b < blocks.Length; b++)
            {
                for (int c = 0; c < blocks.Length; c++)
                {
                    for (int d = 0; d < blocks.Length; d++)
                    {
                        for (int e = 0; e < blocks.Length; e++)
                        {
                            for (int f = 0; f < blocks.Length; f++)
                            {
                                for (int g = 0; g < blocks.Length; g++)
                                {
                                    if(CountNotNull(strips)==0)
                                    {
                                        Debug.Log(" Strips are end; ");
                                        return;
                                    }

                                    if (CountNotNull(blocks) == 0)
                                    {
                                        Debug.Log(" Blocks are end; ");
                                        return;
                                    } 
                                    
                                    if(blocks[g] == null) continue;
                                    if (Math.Round(blocks[g].transform.localScale.x, 3) ==
                                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                                    {
                                        sortingBlocks[0] = blocks[g];
                                        blocks[g] = null;
                                        AddToStrip();
                                        continue;
                                    }
                                    
                                    if(blocks[f] == null) continue;
                                    if(f == g) continue;
                                    if (Math.Round(blocks[f].transform.localScale.x, 3)+
                                        Math.Round(blocks[g].transform.localScale.x, 3) ==
                                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                                    {
                                        sortingBlocks[0] = blocks[g];
                                        sortingBlocks[1] = blocks[f];
                                        blocks[g] = null;
                                        blocks[f] = null;
                                        AddToStrip();
                                        continue;
                                    }
                                    
                                    if(blocks[e] == null) continue;
                                    if(e == f || e== g) continue;
                                    if (Math.Round(blocks[e].transform.localScale.x, 3)+
                                        Math.Round(blocks[f].transform.localScale.x, 3)+
                                        Math.Round(blocks[g].transform.localScale.x, 3) ==
                                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                                    {
                                        sortingBlocks[0] = blocks[g];
                                        sortingBlocks[1] = blocks[f];
                                        sortingBlocks[2] = blocks[e];
                                        blocks[g] = null;
                                        blocks[f] = null;
                                        blocks[e] = null;
                                        AddToStrip();
                                        continue;
                                    }
                                    
                                    if(blocks[d] == null) continue;
                                    if(d==e || d == f || d == g) continue;
                                    if (Math.Round(blocks[d].transform.localScale.x, 3)+
                                        Math.Round(blocks[e].transform.localScale.x, 3)+
                                        Math.Round(blocks[f].transform.localScale.x, 3)+
                                        Math.Round(blocks[g].transform.localScale.x, 3) ==
                                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                                    {
                                        sortingBlocks[0] = blocks[g];
                                        sortingBlocks[1] = blocks[f];
                                        sortingBlocks[2] = blocks[e];
                                        sortingBlocks[3] = blocks[d];
                                        
                                        blocks[g] = null;
                                        blocks[f] = null;
                                        blocks[e] = null;
                                        blocks[d] = null;
                                        AddToStrip();
                                        continue;
                                    }
                                    
                                    if(blocks[c] == null) continue;
                                    if(c == d || c==e || c == f || c == g) continue;
                                    if (Math.Round(blocks[c].transform.localScale.x, 3)+
                                        Math.Round(blocks[d].transform.localScale.x, 3)+
                                        Math.Round(blocks[e].transform.localScale.x, 3)+
                                        Math.Round(blocks[f].transform.localScale.x, 3)+
                                        Math.Round(blocks[g].transform.localScale.x, 3) ==
                                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                                    {
                                        sortingBlocks[0] = blocks[g];
                                        sortingBlocks[1] = blocks[f];
                                        sortingBlocks[2] = blocks[e];
                                        sortingBlocks[3] = blocks[d];
                                        sortingBlocks[4] = blocks[c];
                                        
                                        blocks[g] = null;
                                        blocks[f] = null;
                                        blocks[e] = null;
                                        blocks[d] = null;
                                        blocks[c] = null;
                                        AddToStrip();
                                        continue;
                                    }
                                    
                                    if(blocks[b] == null) continue;
                                    if(b == c || b == d || b==e || b == f || b == g) continue;
                                    if (Math.Round(blocks[b].transform.localScale.x, 3)+
                                        Math.Round(blocks[c].transform.localScale.x, 3)+
                                        Math.Round(blocks[d].transform.localScale.x, 3)+
                                        Math.Round(blocks[e].transform.localScale.x, 3)+
                                        Math.Round(blocks[f].transform.localScale.x, 3)+
                                        Math.Round(blocks[g].transform.localScale.x, 3) ==
                                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                                    {
                                        sortingBlocks[0] = blocks[g];
                                        sortingBlocks[1] = blocks[f];
                                        sortingBlocks[2] = blocks[e];
                                        sortingBlocks[3] = blocks[d];
                                        sortingBlocks[4] = blocks[c];
                                        sortingBlocks[5] = blocks[b];
                                        
                                        blocks[g] = null;
                                        blocks[f] = null;
                                        blocks[e] = null;
                                        blocks[d] = null;
                                        blocks[c] = null;
                                        blocks[b] = null;
                                        
                                        AddToStrip();
                                        continue;
                                    }
                                    
                                    if(blocks[a] == null) continue;
                                    if(a == b || a == c || a == d || a==e || a == f || a == g) continue;
                                    if (Math.Round(blocks[a].transform.localScale.x, 3)+
                                        Math.Round(blocks[b].transform.localScale.x, 3)+
                                        Math.Round(blocks[c].transform.localScale.x, 3)+
                                        Math.Round(blocks[d].transform.localScale.x, 3)+
                                        Math.Round(blocks[e].transform.localScale.x, 3)+
                                        Math.Round(blocks[f].transform.localScale.x, 3)+
                                        Math.Round(blocks[g].transform.localScale.x, 3) ==
                                        Math.Round(strips[sortingStrip].GetComponent<Strip>().leftLength, 3))
                                    {
                                        sortingBlocks[0] = blocks[g];
                                        sortingBlocks[1] = blocks[f];
                                        sortingBlocks[2] = blocks[e];
                                        sortingBlocks[3] = blocks[d];
                                        sortingBlocks[4] = blocks[c];
                                        sortingBlocks[5] = blocks[b];
                                        sortingBlocks[6] = blocks[a];
                                        
                                        blocks[g] = null;
                                        blocks[f] = null;
                                        blocks[e] = null;
                                        blocks[d] = null;
                                        blocks[c] = null;
                                        blocks[b] = null;
                                        blocks[a] = null;
                                        
                                        AddToStrip();
                                        continue;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        Debug.Log(sortingStrip + " strip not filled try next");
        emptyStrips[numOfEmptyStrips] = strips[sortingStrip];
        strips[sortingStrip] = null;
        numOfEmptyStrips++;
        sortingStrip++;
        if(CountNotNull(strips)==0)
        {
            Debug.Log(" Strips are end; ");
            return;
        }

        if (CountNotNull(blocks) == 0)
        {
            Debug.Log(" Blocks are end; ");
            return;
        } 
        SortMaxLength();
    }

    void AddToStrip()
    {
        strips[sortingStrip].GetComponent<Strip>().leftLength = 0;

        for (int b = 0; b < sortingBlocks.Length; b++) // Заполняем балку(с идеальным заполнением) блоками
        {
            if (sortingBlocks[b] == null) continue;
            sortingBlocks[b].transform.SetParent(strips[sortingStrip].transform);
            sortingBlocks[b].transform.localPosition = 
                new Vector3(b * (sortingBlocks[b].transform.localScale.x + 1), 0, 0);
            sortingBlocks[b] = null;
        }

        doneStrips[numOfDoneStrips] = strips[sortingStrip];
        strips[sortingStrip] = null;
            
        numOfDoneStrips++;
        sortingStrip++;
        sortingLengthBlocks = 0;
        return;
    }
    
    int MaxBlock(int exception = -1)
    {
        GameObject max = minPrefab;
        int maxInt = 0;
        for (int i = 0; i < blocks.Length; i++)
        { // Установить случайные данные для сравнения и исключения ошибок
            if (blocks[i] != null)
            {
                max = blocks[i];
                maxInt = i;
                break;
            }

            if (i == blocks.Length - 1 && max == minPrefab)
            {
                return (-1);
            }
        }
        
        
        for (int i = 0; i < blocks.Length; i++)
        { // Вычисление максимума при возможном искючении "exception"
            if (blocks[i] != null && i != exception && blocks[i].transform.localScale.x > max.transform.localScale.x)
            {
                max = blocks[i];
                maxInt = i;
            }
            
        }

        return (maxInt);
    }

    int MinBlock(int exception = -1)
    {
        GameObject min = maxPrefab;
        int minInt = 0;
        for (int i = 0; i < blocks.Length; i++)
        { // Установить случайные данные для сравнения и исключения ошибок
            if (blocks[i] != null)
            {
                min = blocks[i];
                minInt = i;
                break;
            }

            if (i == blocks.Length - 1 && min == maxPrefab)
            {
                return (-1);
            }
        }
        
        
        for (int i = 0; i < blocks.Length; i++)
        { // Вычисление максимума при возможном искючении "exception"
            if (blocks[i] != null && i != exception && blocks[i].transform.localScale.x < min.transform.localScale.x)
            {
                min = blocks[i];
                minInt = i;
            }
            
        }

        return (minInt);
    }
    int CountNotNull(GameObject[] arr)
    {
        int count = 0;
        foreach (GameObject gO in arr)
        {
            if (gO != null) count++;
        }

        return (count);
    }
}
