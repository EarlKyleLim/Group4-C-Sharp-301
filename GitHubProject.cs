using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubProject
{
    class Question
    {
        public string Text { get; set; }
        public List<string> Choices { get; set; }
        public string CorrectAnswer { get; set; }

        public Question(string text, List<string> choices, string correctAnswer)
        {
            Text = text;
            Choices = choices;
            CorrectAnswer = correctAnswer;
        }
    }

    class Quiz
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }

        public Quiz(string name)
        {
            Name = name;
            Questions = new List<Question>();
        }
    }

    class Program
    {
        static List<Quiz> quizzes = new List<Quiz>();
        static string saveFilePath = "quizzes.txt";

        static void Main()
        {
            LoadQuizzes();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Quiz Game");
                Console.WriteLine("1. Create a new quiz");
                Console.WriteLine("2. Take an existing quiz");
                Console.WriteLine("3. View all quizzes");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateQuiz();
                        break;
                    case "2":
                        TakeQuiz();
                        break;
                    case "3":
                        ViewQuizzes();
                        break;
                    case "4":
                        SaveQuizzes();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void CreateQuiz()
        {
            Console.Clear();
            Console.Write("Enter the name of the quiz: ");
            string quizName = Console.ReadLine();
            Quiz newQuiz = new Quiz(quizName);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Creating questions for quiz: " + quizName);
                Console.Write("Enter the question text: ");
                string questionText = Console.ReadLine();

                List<string> choices = new List<string>();
                for (int i = 0; i < 4; i++)
                {
                    Console.Write($"Enter choice {i + 1}: ");
                    choices.Add(Console.ReadLine());
                }

                Console.Write("Enter the correct answer: ");
                string correctAnswer = Console.ReadLine();

                newQuiz.Questions.Add(new Question(questionText, choices, correctAnswer));

                Console.Write("Add another question? (y/n): ");
                if (Console.ReadLine().ToLower() != "y") break;
            }

            quizzes.Add(newQuiz);
            Console.WriteLine("Quiz created successfully! Press any key to return to the main menu...");
            Console.ReadKey();
        }

        static void TakeQuiz()
        {
            Console.Clear();
            if (quizzes.Count == 0)
            {
                Console.WriteLine("No quizzes available. Press any key to return to the main menu...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Quizzes:");
            for (int i = 0; i < quizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {quizzes[i].Name}");
            }
            Console.Write("Select a quiz by number: ");
            if (int.TryParse(Console.ReadLine(), out int quizIndex) && quizIndex > 0 && quizIndex <= quizzes.Count)
            {
                Quiz selectedQuiz = quizzes[quizIndex - 1];
                int score = 0;

                foreach (var question in selectedQuiz.Questions)
                {
                    Console.Clear();
                    Console.WriteLine($"Question: {question.Text}");
                    for (int i = 0; i < question.Choices.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {question.Choices[i]}");
                    }
                    Console.Write("Your answer: ");
                    string userAnswer = Console.ReadLine();

                    if (userAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Correct!");
                        score++;
                    }
                    else
                    {
                        Console.WriteLine($"Wrong! The correct answer is {question.CorrectAnswer}");
                    }
                    Console.WriteLine("Press any key for the next question...");
                    Console.ReadKey();
                }

                Console.WriteLine($"You scored {score}/{selectedQuiz.Questions.Count}");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid choice. Press any key to try again...");
                Console.ReadKey();
            }
        }

        static void ViewQuizzes()
        {
            Console.Clear();
            if (quizzes.Count == 0)
            {
                Console.WriteLine("No quizzes available. Press any key to return to the main menu...");
            }
            else
            {
                Console.WriteLine("Available Quizzes:");
                foreach (var quiz in quizzes)
                {
                    Console.WriteLine("- " + quiz.Name);
                }
                Console.WriteLine("Press any key to return to the main menu...");
            }
            Console.ReadKey();
        }

        static void SaveQuizzes()
        {
            using (StreamWriter writer = new StreamWriter(saveFilePath))
            {
                foreach (var quiz in quizzes)
                {
                    writer.WriteLine(quiz.Name);
                    foreach (var question in quiz.Questions)
                    {
                        writer.WriteLine(question.Text);
                        writer.WriteLine(string.Join(";", question.Choices));
                        writer.WriteLine(question.CorrectAnswer);
                    }
                    writer.WriteLine("---");
                }
            }
        }

        static void LoadQuizzes()
        {
            if (!File.Exists(saveFilePath)) return;

            using (StreamReader reader = new StreamReader(saveFilePath))
            {
                string line;
                Quiz currentQuiz = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "---")
                    {
                        if (currentQuiz != null) quizzes.Add(currentQuiz);
                        currentQuiz = null;
                    }
                    else if (currentQuiz == null)
                    {
                        currentQuiz = new Quiz(line);
                    }
                    else
                    {
                        string questionText = line;
                        string[] choices = reader.ReadLine().Split(';');
                        string correctAnswer = reader.ReadLine();
                        currentQuiz.Questions.Add(new Question(questionText, new List<string>(choices), correctAnswer));
                    }
                }

                if (currentQuiz != null) quizzes.Add(currentQuiz);
            }
        }
    }
}
/* Sir dunno saon ni pag run sa Visual Studio na Blue pero ayos mn pag sa purple tung 2022 na Visual Studio*/
