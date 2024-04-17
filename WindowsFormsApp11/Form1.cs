using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    // Класс Form1, представляющий главную форму приложения  
    public partial class Form1 : Form
    {
        // Поле для хранения серии геометрических фигур  
        private Series series = new Series();

        // Конструктор для инициализации формы  
        public Form1()
        {
            InitializeComponent();
        }

        // Обработчик события нажатия на кнопку "Добавить"  
        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                double length = double.Parse(textBoxLength.Text);
                double width = double.Parse(textBoxWidth.Text);
                double height = double.Parse(textBoxHeight.Text);

                Parallelepiped parallelepiped = new Parallelepiped(length, width, height);
                series.AddBody(parallelepiped);

                RefreshCharacteristicsDisplay();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for length, width, and height.");
            }
        }

        // Обработчик события нажатия на кнопку "Сортировать"  
        private void sortButton_Click(object sender, EventArgs e)
        {
            series.SortByVolume();
            RefreshCharacteristicsDisplay();
        }

        // Обработчик события нажатия на кнопку "Экспорт в файл"  
        private void exportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    series.ExportToFile(saveFileDialog.FileName);
                }
            }
        }

        // Метод для обновления отображения характеристик в ListBox  
        private void RefreshCharacteristicsDisplay()
        {
            listBoxCharacteristics.Items.Clear();

            foreach (var body in series.GetBodies())
            {
                listBoxCharacteristics.Items.Add($"Surface Area: {body.SurfaceArea()}, Volume: {body.Volume()}");
                body.Draw(pictureBox1); // Добавляем отрисовку фигуры на PictureBox  
            }
        }
    }

    // Абстрактный класс, представляющий геометрическое тело  
    public abstract class Body
    {
        // Абстрактный метод для вычисления площади поверхности  
        public abstract double SurfaceArea();

        // Абстрактный метод для вычисления объема  
        public abstract double Volume();

        // Абстрактный метод для отрисовки фигуры на PictureBox  
        public abstract void Draw(PictureBox pictureBox);
    }

    // Класс Parallelepiped, представляющий параллелепипед  
    public class Parallelepiped : Body
    {
        // Поля для хранения размеров параллелепипеда  
        private double Length { get; }
        private double Width { get; }
        private double Height { get; }

        // Конструктор для инициализации размеров параллелепипеда  
        public Parallelepiped(double length, double width, double height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        // Метод для вычисления площади поверхности параллелепипеда  
        public override double SurfaceArea()
        {
            return 2 * (Length * Width + Length * Height + Width * Height);
        }

        // Метод для вычисления объема параллелепипеда  
        public override double Volume()
        {
            return Length * Width * Height;
        }

        // Метод для отрисовки параллелепипеда на PictureBox  
        public override void Draw(PictureBox pictureBox)
        {
            Graphics g = pictureBox.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            g.DrawRectangle(pen, 10, 10, (float)Length, (float)Width);
        }
    }

    // Класс Cone, представляющий конус  
    public class Cone : Body
    {
        // Поля для хранения радиуса и высоты конуса  
        private double Radius { get; }
        private double Height { get; }

        // Конструктор для инициализации радиуса и высоты конуса  
        public Cone(double radius, double height)
        {
            Radius = radius;
            Height = height;
        }

        // Метод для вычисления площади поверхности конуса  
        public override double SurfaceArea()
        {
            return Math.PI * Radius * (Radius + Math.Sqrt(Radius * Radius + Height * Height));
        }

        // Метод для вычисления объема конуса  
        public override double Volume()
        {
            return (1.0 / 3.0) * Math.PI * Radius * Radius * Height;
        }

        // Метод для отрисовки конуса на PictureBox  
        public override void Draw(PictureBox pictureBox)
        {
            Graphics g = pictureBox.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            g.DrawEllipse(pen, 10, 10, (float)Radius * 2, (float)Radius * 2);
            g.DrawLine(pen, 10 + (float)Radius, 10 + (float)Radius * 2, 10 + (float)Radius, 10 + (float)Height);
        }
    }

    // Класс Ball, представляющий сферу  
    public class Ball : Body
    {
        // Поле для хранения радиуса сферы  
        private double Radius { get; }

        // Конструктор для инициализации радиуса сферы  
        public Ball(double radius)
        {
            Radius = radius;
        }

        // Метод для вычисления площади поверхности сферы  
        public override double SurfaceArea()
        {
            return 4 * Math.PI * Radius * Radius;
        }

        // Метод для вычисления объема сферы  
        public override double Volume()
        {
            return (4.0 / 3.0) * Math.PI * Radius * Radius * Radius;
        }

        // Метод для отрисовки сферы на PictureBox  
        public override void Draw(PictureBox pictureBox)
        {
            Graphics g = pictureBox.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            g.DrawEllipse(pen, 10, 10, (float)Radius * 2, (float)Radius * 2);
        }
    }

    // Класс Series, представляющий серию геометрических фигур  
    public class Series
    {
        // Список для хранения геометрических фигур  
        private List<Body> bodies = new List<Body>();

        // Метод для добавления геометрической фигуры в серию  
        public void AddBody(Body body)
        {
            bodies.Add(body);
        }

        // Метод для получения списка геометрических фигур  
        public List<Body> GetBodies()
        {
            return bodies;
        }

        // Метод для сортировки геометрических фигур по объему  
        public void SortByVolume()
        {
            bodies.Sort((a, b) => a.Volume().CompareTo(b.Volume()));
        }

        // Метод для экспорта информации о геометрических фигурах в файл  
        public void ExportToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var body in bodies)
                {
                    writer.WriteLine($"Surface Area: {body.SurfaceArea()}, Volume: {body.Volume()}");
                }
            }
        }

        // Метод для импорта информации о геометрических фигурах из файла  
        public void ImportFromFile(string filePath)
        {
            bodies.Clear();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Парсинг строк и создание объектов тел тут...  
                }
            }
        }
    }
}
