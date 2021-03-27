using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class APNodePool
{
    private APNodePool() { }
    private static APNodePool _instance;
    public static APNodePool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new APNodePool();

            return _instance;
        }
    }
    public enum NodeType { Move, Attack, Item, Skill };

    // 노드의 타입으로 노드의 리스트를 찾아갈 수 있는 딕셔너리
    // 그리고 각 노드와 dirty bit의 pair를 저장합니다.
    // dirty bit가 true이면 Plan에 사용중인 Node
    // false이면 새로 사용가능한 Node
    private Dictionary<NodeType, Dictionary<APActionNode, bool>> pool = 
        new Dictionary<NodeType, Dictionary<APActionNode, bool>>();
    
    public bool GetNode(NodeType type, out APActionNode newNode)
    {
        // 첫 접근
        if(pool.Count == 0)
        {
            pool.Add(NodeType.Move, new Dictionary<APActionNode, bool>());
            pool.Add(NodeType.Attack, new Dictionary<APActionNode, bool>());
            pool.Add(NodeType.Item, new Dictionary<APActionNode, bool>());
            pool.Add(NodeType.Skill, new Dictionary<APActionNode, bool>());
        }


        Dictionary<APActionNode, bool> dic = pool[type];

        foreach(var node in dic.Keys)
        {
            if(dic[node] == false)
            {
                dic[node] = true;
                newNode = node;
                return true;
            }
        }

        newNode = null;
        return false;
    }

    public void AddNode(APActionNode newNode)
    {
        if(newNode is ActionNode_Move)
        {
            pool[NodeType.Move].Add(newNode, false);
        }
        else if(newNode is ActionNode_Attack)
        {
            pool[NodeType.Attack].Add(newNode, false);
        }

        // TODO
        //else if (newNode is ActionNode_Item)
        //{
            //pool[NodeType.Item].Add(newNode, false);
        //}
        //else if (newNode is ActionNode_Skill)
        //{
            //pool[NodeType.Skill].Add(newNode, false);  
        //}

    }

    public void Reset()
    {
        var listOfDics = pool.Values.ToList();
        for (int i = 0; i < listOfDics.Count; i++)
        {
            var nodes = listOfDics[i].Keys.ToList();

            Parallel.ForEach(nodes, node => listOfDics[i][node] = false);
        }
            
    }

}

public class APGameStatePool
{
    private APGameStatePool() { }
    private static APGameStatePool _instance;
    public static APGameStatePool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new APGameStatePool();

            return _instance;
        }
    }
    private Dictionary<APGameState, bool> pool = new Dictionary<APGameState, bool>();
    public void GetState(out APGameState newState)
    {
        foreach (var state in pool.Keys)
        {
            if (pool[state] == false)
            {
                pool[state] = true;
                newState = state;
                return;
            }
        }

        newState = new APGameState();
        pool.Add(newState, true);
    }

    public void Reset()
    {
        var keys = pool.Keys.ToList();
        Parallel.ForEach(keys, key => pool[key] = false);
    }

}
