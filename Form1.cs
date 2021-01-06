using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace My_Notepad
{
    public partial class Form1 : Form
    {
        StackUsingLinkedlist mainStack, redoStack;
        int stackCount = 0;
        int spaceCount;
        char[] words;
        string[] input;
        string file;
        string[] fileSplit;
        public Form1()
        {
            InitializeComponent();
            mainStack = new StackUsingLinkedlist();
            redoStack = new StackUsingLinkedlist();
            loadFile();
        }


        void loadFile()
        {
            using (StreamReader sr = new StreamReader("prediction.txt"))
            {
                file = sr.ReadToEnd();
                fileSplit = file.Split();
                sr.Close();
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Title = "My open file dialog";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Clear();
                using (StreamReader sr = new StreamReader(openfile.FileName))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
        }

        // this saves richtextbox1 to a file
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save file as..";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter txtoutput = new StreamWriter(savefile.FileName);
                txtoutput.Write(richTextBox1.Text);
                txtoutput.Close();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int length;
            string last;
         
            listBox1.Visible = false;
            listBox1.Items.Clear();

            spaceCount = -1;

            words = richTextBox1.Text.ToCharArray();

            for (int i=0; i<words.Length; i++)
            {
                if (words[i] == ' ')
                    spaceCount++;
            }

            if (spaceCount == stackCount)
            {
                input = richTextBox1.Text.Split();
                mainStack.push(input[spaceCount] + " ");
                stackCount++;

                // until here the stack is pushed

                var pos = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
                listBox1.Location = new Point(pos.X, pos.Y+65 );

                length = input.Length - 2;
                last = input[length];


                for (int i = 0; i < fileSplit.Length; i++)
                {
                    if (last == fileSplit[i])
                    {
                        listBox1.Visible = true;
                        listBox1.Items.Add(fileSplit[i + 1]);
                    }

                }
                // until here word prediction is done

            }

           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string[] words = textBox1.Text.Split(',');
                foreach (string word in words)
                {
                    int startindex = 0;
                    while (startindex < richTextBox1.TextLength)
                    {
                        int wordstartIndex = richTextBox1.Find(word, startindex, RichTextBoxFinds.None);
                        if (wordstartIndex != -1)
                        {
                            richTextBox1.SelectionStart = wordstartIndex;
                            richTextBox1.SelectionLength = word.Length;
                            richTextBox1.SelectionBackColor = Color.Yellow;
                        }
                        else
                            break;
                        startindex += wordstartIndex + word.Length;
                    }
                }
            }

            else
            {
                richTextBox1.SelectionStart = 0;
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = Color.White;
            }
        }


        void undo()
        {
            if (richTextBox1.Text != "")
            {
                if (mainStack != null)
                    redoStack.push(mainStack.pop());

                string[] a = richTextBox1.Text.Split();
                string lastWord = a[a.Length - 2];

                int sizetxt = richTextBox1.Text.Length;
                int sizeword = lastWord.Length;

                int remaining = sizetxt - sizeword;

                if (richTextBox1.Text != "")
                    richTextBox1.Text = richTextBox1.Text.Remove(remaining - 1);
            }

        }

        void redo()
        {
            if (redoStack != null)
            {
                richTextBox1.Text += redoStack.pop();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text += listBox1.Items[listBox1.SelectedIndex].ToString();
            richTextBox1.SelectionStart = richTextBox1.Text.Length;

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undo();
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            undo();
        }

       

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redo();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            redo();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveAsToolStripMenuItem.PerformClick();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cutToolStripButton.PerformClick();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripButton.PerformClick();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripButton.PerformClick();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        public class StackUsingLinkedlist
        {

            // A linked list node  
            public class Node
            {
                // integer data  
                public string data;

                // reference variable Node type  
                public Node link;
            }

            // create global top reference variable  
            public Node top;

            // Constructor  
            public StackUsingLinkedlist()
            {
                this.top = null;
            }

            // Utility function to add  
            // an element x in the stack  
            // insert at the beginning  
            public void push(string x)
            {
                // create new node temp and allocate memory  
                Node temp = new Node();

                // check if stack (heap) is full.  
                // Then inserting an element 
                // would lead to stack overflow  
                if (temp == null)
                {
                    return;
                }

                // initialize data into temp data field  
                temp.data = x;

                // put top reference into temp link  
                temp.link = top;

                // update top reference  
                top = temp;
            }

            // Utility function to check if 
            // the stack is empty or not  
            public bool isEmpty()
            {
                return top == null;
            }

            // Utility function to return 
            // top element in a stack  
            public string peek()
            {
                // check for empty stack  
                if (!isEmpty())
                {
                    return top.data;
                }
                else
                {
                    Console.WriteLine("Stack is empty");
                    return "";
                }
            }

            // Utility function to pop top element from the stack  
            public string pop() // remove at the beginning  
            {
                string info;
                // check for stack underflow  
                if (top == null)
                {
                    return "";
                }

                // update the top pointer to  
                // point to the next node
                else
                {
                    info = top.data;
                    top = (top).link;
                    return info;
                }
            }

            public void display()
            {
                // check for stack underflow  
                if (top == null)
                {
                    Console.Write("\nStack Underflow");
                    return;
                }
                else
                {
                    Node temp = top;
                    while (temp != null)
                    {

                        // print node data  
                        Console.Write("{0}->", temp.data);

                        // assign temp link to temp  
                        temp = temp.link;
                    }
                }
            }
        }

      
    }

   
}
