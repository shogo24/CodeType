using UnityEngine;

public class CodeGenerator : MonoBehaviour
{
    public string GenerateCode()
    {
        return GetPart1()
             + GetFields()
             + GetPart2()
             + GetBody()
             + GetOptionalElse()
             + GetPart4();
    }

    string GetPart1()
    {
        string[] options =
        {
            "using·UnityEngine;\n\npublic·class·Player·:·MonoBehaviour\n{\n",
            "using·UnityEngine;\n\npublic·class·Enemy·:·MonoBehaviour\n{\n",
            "using·UnityEngine;\n\npublic·class·GameManager·:·MonoBehaviour\n{\n",
            "public·class·Inventory\n{\n",
            "public·class·Controller\n{\n"
        };

        return options[Random.Range(0, options.Length)];
    }

    string GetFields()
    {
        string[] fields =
        {
            "    int·health·=·100;\n",
            "    float·speed·=·5f;\n",
            "    bool·isAlive·=·true;\n",
            "    int·score;\n",
            "    Transform·target;\n"
        };

        int count = Random.Range(1, 3);
        string result = "";

        for (int i = 0; i < count; i++)
            result += fields[Random.Range(0, fields.Length)];

        result += "\n";
        return result;
    }

    string GetPart2()
    {
        string[] options =
        {
            "    void·Update()\n    {\n        if·(isAlive)\n        {\n",
            "    void·Start()\n    {\n        if·(health·<=·0)\n        {\n",
            "    void·FixedUpdate()\n    {\n        if·(speed·>·0)\n        {\n"
        };

        return options[Random.Range(0, options.Length)];
    }

    string GetBody()
    {
        string[] lines =
        {
            "            Debug.Log(\"Hello·World\");\n",
            "            Debug.Log(\"Running\");\n",
            "            health·-=·10;\n",
            "            speed·-=·Time.deltaTime;\n",
            "            score·+=·10;\n",
            "            isAlive·=·false;\n"
        };

        string[] comments =
        {
            "            //·Update·player·state\n",
            "            //·Apply·damage\n",
            "            //·Move·object\n",
            "            //·Check·conditions\n"
        };

        int count = Random.Range(1, 4);
        string result = "";

        // Optional comment
        if (Random.value < 0.5f)
            result += comments[Random.Range(0, comments.Length)];

        for (int i = 0; i < count; i++)
            result += lines[Random.Range(0, lines.Length)];

        return result;
    }

    string GetOptionalElse()
    {
        float roll = Random.value;

        if (roll < 0.33f)
            return ""; // no else

        if (roll < 0.66f)
        {
            return
                "        }\n" +
                "        else\n" +
                "        {\n" +
                "            Debug.Log(\"Else·Block\");\n";
        }

        return
            "        }\n" +
            "        else·if·(health·<·50)\n" +
            "        {\n" +
            "            Debug.Log(\"Low·Health\");\n";
    }

    string GetPart4()
    {
        return
            "        }\n" +
            "    }\n" +
            "}\n";
    }
}
