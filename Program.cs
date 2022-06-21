class Utils {
    public char winner(char [,] field) {
        int N = field.GetLength(0); 
        foreach (char c in new List<int> {'X', 'O'}) {
            bool ok;
            for(int i = 0; i < N; i++) {
                ok = true; 
                for(int j = 0; j < N; j++) ok = ok && (field[i,j] == c);
                if (ok) return c;
            }
            for(int i = 0; i < N; i++) {
                ok = true; 
                for(int j = 0; j < N; j++) ok = ok && (field[j,i] == c);
                if (ok) return c;
            }
            ok = true;
            for(int k = 0; k < N; k++) ok = ok && (field[k,k] == c);
            if (ok) return c;
            ok = true;
            for(int k = 0; k < N; k++) ok = ok && (field[k,2-k] == c);
            if (ok) return c;
        }
        for(int i = 0; i < N; i++) {
            for(int j = 0; j < N; j++) if (field[i,j] == '.') return '-';
        }
        return 'D';
    } 

    public List<char[,]> possibleMoves(char[,] field, char turn) {
        int N = field.GetLength(0);
        var res = new List<char[,]>();
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
                if(field[i,j] == '.') {
                    char[,] a = field.Clone() as char[,];
                    a[i,j] = turn;
                    res.Add(a); 
                }
            }
        }
        return res; 
    }

    public int[] dfs(char[,] field, char turn, Dictionary < char[,] , int[] > stats, Dictionary < char[,] , char[,] > bestMove) {
        char win = winner(field);
        if (stats.ContainsKey(field)) return stats[field]; 
        if (win == 'X') {
            stats[field] = new int[] {1,0,0};  
            return stats[field]; 
        }
        if (win == 'O') {
            stats[field] = new int[] {0,1,0};  
            return stats[field]; 
        }
        if (win == 'D') {
            stats[field] = new int[] {0,0,1};  
            return stats[field]; 
        } 
        
        List<char[,]> moves = possibleMoves(field, turn);
        int [] res = new int[3];
        int N = field.GetLength(0);
        char [,] choice = new char [N, N]; 
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) choice[i,j] = 'n';
        }
        int maxChance = -1; 
        foreach(char [,] nextField in moves) {
            int[] prev = dfs(nextField, (turn == 'X' ? 'O' : 'X'), stats, bestMove);
            for (int i = 0; i < 3; i++) res[i] += prev[i];
            if (prev[0] > maxChance)
            if (turn == 'X' && prev[0] > maxChance) {
                maxChance = prev[0];
                for (int i = 0; i < N; i++) {
                    for (int j = 0; j < N; j++) choice[i,j] = nextField[i,j];
                }
            }
            if (turn == 'O' && prev[1] > maxChance) {
                maxChance = prev[1];
                for (int i = 0; i < N; i++) {
                    for (int j = 0; j < N; j++) choice[i,j] = nextField[i,j];
                }
            }
        }
        if (choice[0,0] != 'n') bestMove[field] = choice;
        else if (moves.Count > 0) bestMove[field] = moves[0]; 
        stats[field] = res;
        return res; 
    }

    public void printToFile(string filename, Dictionary < char[,] , char[,] > bestMove) {
        using (var sw = new StreamWriter(filename)) {
            foreach(KeyValuePair<char[,], char[,]> entry in bestMove) { 
                int N = entry.Key.GetLength(0);   
                for (int i = 0; i < N; i++) {
                    for (int j = 0; j < N; j++) sw.Write(entry.Key[i,j]);
                    sw.Write("\n");
                }
                sw.Write("\n");
                for (int i = 0; i < N; i++) {
                    for (int j = 0; j < N; j++) sw.Write(entry.Value[i,j]);
                    sw.Write("\n");
                }
                sw.Write("\n");
            }
            sw.Flush();
            sw.Close();
        }
    }
}

class Hello {     
    
    static void Main(string[] args)
    {
        Utils utils = new Utils();
        int N = 3;
        char[,] field = new char[N, N];
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) field[i,j] = '.';
        }
        Dictionary < char[,] , int[] > stats = new Dictionary<char[,], int[]>(); 
        Dictionary < char[,] , char[,] > bestMove = new Dictionary<char[,], char[,]>(); 
        int[] res = utils.dfs(field, 'X', stats, bestMove); 
        for (int i = 0; i < 3; i++) {
            Console.Write(res[i]);
            Console.Write(" ");
        }
        Console.Write("\n"); 
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
                Console.Write(bestMove[field][i,j]);
            }
            Console.Write("\n"); 
        }
        utils.printToFile("bestMoves.txt", bestMove);
        Console.Write(bestMove.Count);
    }
}

 