using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using Timer = System.Windows.Forms.Timer;

namespace MusteriYonetim
{
    public partial class Form1 : Form
    {
        // MySQL bağlantı string'i
        string connectionString = "Server=localhost;Database=db;Uid=root;Pwd=;";
        
        // Renk şeması
        Color primaryColor = Color.FromArgb(41, 128, 185);      // Mavi
        Color secondaryColor = Color.FromArgb(52, 73, 94);      // Koyu gri
        Color accentColor = Color.FromArgb(46, 204, 113);       // Yeşil
        Color dangerColor = Color.FromArgb(231, 76, 60);        // Kırmızı
        Color bgColor = Color.FromArgb(236, 240, 241);          // Açık gri
        Color textColor = Color.FromArgb(44, 62, 80);           // Koyu text
        
        // Form elemanları
        TextBox? txtId, txtIsim;
        ComboBox? cmbKonu;
        RichTextBox? rtbMesaj, rtbDosya;
        Button? btnKaydet, btnSil, btnGuncelle, btnAra;
        Button? btnDosyaKaydet, btnDosyaAc;
        ProgressBar? progressBar1;
        Timer? timer1;
        ListBox? listBox1;
        SaveFileDialog? saveFileDialog1;
        OpenFileDialog? openFileDialog1;
        Panel? headerPanel, mainPanel, sidePanel;
        
        public Form1()
        {
            InitializeComponent();
            ModernFormOlustur();
        }
        
        void ModernFormOlustur()
        {
            // Form ayarları
            this.Text = "Müşteri Yönetim Sistemi - Modern UI";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = bgColor;
            this.FormBorderStyle = FormBorderStyle.None;
            
            // Header Panel
            headerPanel = new Panel
            {
                Size = new Size(1200, 60),
                Location = new Point(0, 0),
                BackColor = primaryColor
            };
            
            Label lblBaslik = new Label
            {
                Text = "🏢 MÜŞTERİ YÖNETİM SİSTEMİ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            
            // Kapat butonu
            Button btnKapat = new Button
            {
                Text = "✕",
                Size = new Size(40, 40),
                Location = new Point(1140, 10),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnKapat.FlatAppearance.BorderSize = 0;
            btnKapat.Click += (s, e) => Application.Exit();
            
            headerPanel.Controls.Add(lblBaslik);
            headerPanel.Controls.Add(btnKapat);
            
            // Ana panel - Sol taraf (Müşteri İşlemleri)
            mainPanel = new Panel
            {
                Size = new Size(600, 600),
                Location = new Point(20, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            ApplyShadow(mainPanel);
            
            // Başlık
            Label lblMusteriIslemleri = new Label
            {
                Text = "📋 Müşteri İşlemleri",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblMusteriIslemleri);
            
            // ID alanı
            CreateStyledLabel("ID:", new Point(30, 70), mainPanel);
            txtId = CreateStyledTextBox(new Point(150, 70), mainPanel);
            txtId.Width = 80;
            
            // İsim alanı
            CreateStyledLabel("İsim:", new Point(30, 110), mainPanel);
            txtIsim = CreateStyledTextBox(new Point(150, 110), mainPanel);
            
            // Konu alanı
            CreateStyledLabel("Konu:", new Point(30, 150), mainPanel);
            cmbKonu = new ComboBox
            {
                Location = new Point(150, 150),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            cmbKonu.Items.AddRange(new string[] { "🔧 Arıza", "📝 Talep", "❓ Yardım" });
            cmbKonu.SelectedIndex = 0;
            mainPanel.Controls.Add(cmbKonu);
            
            // Mesaj alanı
            CreateStyledLabel("Mesaj:", new Point(30, 190), mainPanel);
            rtbMesaj = new RichTextBox
            {
                Location = new Point(150, 190),
                Size = new Size(400, 120),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            mainPanel.Controls.Add(rtbMesaj);
            
            // Butonlar
            int btnY = 330;
            btnAra = CreateStyledButton("🔍 Ara", new Point(150, btnY), primaryColor);
            btnKaydet = CreateStyledButton("💾 Kaydet", new Point(250, btnY), accentColor);
            btnGuncelle = CreateStyledButton("✏️ Güncelle", new Point(350, btnY), Color.FromArgb(243, 156, 18));
            btnSil = CreateStyledButton("🗑️ Sil", new Point(450, btnY), dangerColor);
            
            btnAra.Click += BtnAra_Click;
            btnKaydet.Click += BtnKaydet_Click;
            btnSil.Click += BtnSil_Click;
            btnGuncelle.Click += BtnGuncelle_Click;
            
            mainPanel.Controls.AddRange(new Control[] { btnAra, btnKaydet, btnGuncelle, btnSil });
            
            // Progress Bar bölümü
            Label lblProgress = new Label
            {
                Text = "⏳ İşlem Durumu (3 saniyede bir güncellenir)",
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor,
                Location = new Point(30, 400),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblProgress);
            
            progressBar1 = new ProgressBar
            {
                Location = new Point(30, 430),
                Size = new Size(520, 25),
                Maximum = 267,
                Style = ProgressBarStyle.Continuous
            };
            mainPanel.Controls.Add(progressBar1);
            
            timer1 = new Timer { Interval = 3000 };
            timer1.Tick += Timer1_Tick;
            timer1.Start();
            
            // Sağ panel
            sidePanel = new Panel
            {
                Size = new Size(540, 600),
                Location = new Point(640, 80),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            ApplyShadow(sidePanel);
            
            // Dosya İşlemleri
            Label lblDosyaIslemleri = new Label
            {
                Text = "📁 Dosya İşlemleri",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            sidePanel.Controls.Add(lblDosyaIslemleri);
            
            rtbDosya = new RichTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(500, 150),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            sidePanel.Controls.Add(rtbDosya);
            
            btnDosyaKaydet = CreateStyledButton("💾 Dosya Kaydet", new Point(20, 220), primaryColor);
            btnDosyaAc = CreateStyledButton("📂 Dosya Aç", new Point(150, 220), secondaryColor);
            
            saveFileDialog1 = new SaveFileDialog { Filter = "Text dosyaları|*.txt", DefaultExt = "txt" };
            openFileDialog1 = new OpenFileDialog { Filter = "Text dosyaları|*.txt" };
            
            btnDosyaKaydet.Click += BtnDosyaKaydet_Click;
            btnDosyaAc.Click += BtnDosyaAc_Click;
            
            sidePanel.Controls.AddRange(new Control[] { btnDosyaKaydet, btnDosyaAc });
            
            // Random Sayılar
            Label lblRandom = new Label
            {
                Text = "🎲 Random Sayılar (50-3000)",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(20, 280),
                AutoSize = true
            };
            sidePanel.Controls.Add(lblRandom);
            
            listBox1 = new ListBox
            {
                Location = new Point(20, 310),
                Size = new Size(200, 120),
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                listBox1.Items.Add(rnd.Next(50, 3001));
            }
            sidePanel.Controls.Add(listBox1);
            
            // Radio Button Grupları
            GroupBox grpCinsiyet = new GroupBox
            {
                Text = "Cinsiyet",
                Location = new Point(240, 310),
                Size = new Size(130, 80),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = textColor
            };
            RadioButton rbErkek = new RadioButton { Text = "👨 Erkek", Location = new Point(10, 25), Checked = true };
            RadioButton rbKadin = new RadioButton { Text = "👩 Kadın", Location = new Point(10, 50) };
            grpCinsiyet.Controls.AddRange(new Control[] { rbErkek, rbKadin });
            
            GroupBox grpSinif = new GroupBox
            {
                Text = "Sınıf",
                Location = new Point(380, 310),
                Size = new Size(140, 120),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = textColor
            };
            RadioButton rb1 = new RadioButton { Text = "1️⃣ 1.sınıf", Location = new Point(10, 25), Checked = true };
            RadioButton rb2 = new RadioButton { Text = "2️⃣ 2.sınıf", Location = new Point(10, 45) };
            RadioButton rb3 = new RadioButton { Text = "3️⃣ 3.sınıf", Location = new Point(10, 65) };
            RadioButton rb4 = new RadioButton { Text = "4️⃣ 4.sınıf", Location = new Point(10, 85) };
            grpSinif.Controls.AddRange(new Control[] { rb1, rb2, rb3, rb4 });
            
            sidePanel.Controls.AddRange(new Control[] { grpCinsiyet, grpSinif });
            
            // MessageBox Test
            Label lblMessageTest = new Label
            {
                Text = "💬 MessageBox Test",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(20, 450),
                AutoSize = true
            };
            sidePanel.Controls.Add(lblMessageTest);
            
            TextBox txtMessageResult = new TextBox
            {
                Location = new Point(20, 480),
                Size = new Size(360, 25),
                Font = new Font("Segoe UI", 10),
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            sidePanel.Controls.Add(txtMessageResult);
            
            Button btnMessageTest = CreateStyledButton("🔔 Test Et", new Point(390, 477), primaryColor);
            btnMessageTest.Size = new Size(100, 30);
            btnMessageTest.Click += (s, e) =>
            {
                var result = MessageBox.Show("Bu işlemi yapmak istediğinize emin misiniz?", "Dikkat",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                txtMessageResult.Text = result == DialogResult.Yes ? 
                    "✅ Kullanıcı Evet'e tıkladı" : "❌ Kullanıcı Hayır'a tıkladı";
            };
            sidePanel.Controls.Add(btnMessageTest);
            
            // Form'a panel'leri ekle
            this.Controls.AddRange(new Control[] { headerPanel, mainPanel, sidePanel });
            
            // Form hareket ettirme
            bool mouseDown = false;
            Point lastLocation = Point.Empty;
            headerPanel.MouseDown += (s, e) => { mouseDown = true; lastLocation = e.Location; };
            headerPanel.MouseMove += (s, e) =>
            {
                if (mouseDown)
                {
                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + e.X,
                        (this.Location.Y - lastLocation.Y) + e.Y);
                    this.Update();
                }
            };
            headerPanel.MouseUp += (s, e) => mouseDown = false;
        }
        
        // Yardımcı metodlar
        private Label CreateStyledLabel(string text, Point location, Panel parent)
        {
            Label lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = textColor,
                Location = location,
                AutoSize = true
            };
            parent.Controls.Add(lbl);
            return lbl;
        }
        
        private TextBox CreateStyledTextBox(Point location, Panel parent)
        {
            TextBox txt = new TextBox
            {
                Location = location,
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(txt);
            return txt;
        }
        
        private Button CreateStyledButton(string text, Point location, Color bgColor)
        {
            Button btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(90, 35),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = bgColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            
            // Hover efekti
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(bgColor, 0.1f);
            btn.MouseLeave += (s, e) => btn.BackColor = bgColor;
            
            return btn;
        }
        
        private void ApplyShadow(Panel panel)
        {
            panel.Paint += (sender, e) =>
            {
                Color[] shadowColors = {
                    Color.FromArgb(30, 0, 0, 0),
                    Color.FromArgb(20, 0, 0, 0),
                    Color.FromArgb(10, 0, 0, 0),
                    Color.FromArgb(5, 0, 0, 0)
                };
                
                for (int i = 0; i < shadowColors.Length; i++)
                {
                    using (Pen pen = new Pen(shadowColors[i]))
                    {
                        e.Graphics.DrawRectangle(pen,
                            new Rectangle(i, i, panel.Width - (i * 2) - 1, panel.Height - (i * 2) - 1));
                    }
                }
            };
        }
        
        // Event Handler'lar (önceki koddan aynı)
        private void BtnAra_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId?.Text))
            {
                MessageBox.Show("Lütfen ID giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM musteriler WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtIsim!.Text = reader["isim"].ToString();
                        cmbKonu!.SelectedIndex = Convert.ToInt32(reader["konu"]);
                        rtbMesaj!.Text = reader["mesaj"].ToString();
                        MessageBox.Show("Kayıt bulundu!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Kayıt bulunamadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Temizle();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIsim?.Text))
            {
                MessageBox.Show("Lütfen isim giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO musteriler (isim, konu, mesaj) VALUES (@isim, @konu, @mesaj)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@isim", txtIsim.Text);
                    cmd.Parameters.AddWithValue("@konu", cmbKonu!.SelectedIndex);
                    cmd.Parameters.AddWithValue("@mesaj", rtbMesaj!.Text);
                    
                    cmd.ExecuteNonQuery();
                    
                    // Eklenen kaydın ID'sini al
                    long insertId = cmd.LastInsertedId;
                    
                    MessageBox.Show($"Kayıt başarıyla eklendi!\nKayıt ID: {insertId}\n\nBu ID'yi kullanarak kaydı arayabilirsiniz.", 
                                "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // ID'yi textbox'a yaz ki kullanıcı görsün
                    txtId!.Text = insertId.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSil_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId?.Text))
            {
                MessageBox.Show("Lütfen silinecek kaydın ID'sini giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bu kaydı silmek istediğinize emin misiniz?", "Onay",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM musteriler WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    
                    int affected = cmd.ExecuteNonQuery();
                    if (affected > 0)
                    {
                        MessageBox.Show("Kayıt silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Temizle();
                    }
                    else
                        MessageBox.Show("Silinecek kayıt bulunamadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnGuncelle_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId?.Text))
            {
                MessageBox.Show("Lütfen güncellenecek kaydın ID'sini giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE musteriler SET isim = @isim, konu = @konu, mesaj = @mesaj WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    cmd.Parameters.AddWithValue("@isim", txtIsim!.Text);
                    cmd.Parameters.AddWithValue("@konu", cmbKonu!.SelectedIndex);
                    cmd.Parameters.AddWithValue("@mesaj", rtbMesaj!.Text);
                    
                    int affected = cmd.ExecuteNonQuery();
                    if (affected > 0)
                        MessageBox.Show("Kayıt güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Güncellenecek kayıt bulunamadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnDosyaKaydet_Click(object? sender, EventArgs e)
        {
            if (saveFileDialog1!.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    rtbDosya!.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                    MessageBox.Show("Dosya kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya kaydedilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void BtnDosyaAc_Click(object? sender, EventArgs e)
        {
            if (openFileDialog1!.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    rtbDosya!.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                    MessageBox.Show("Dosya yüklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya yüklenemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void Timer1_Tick(object? sender, EventArgs e)
        {
            if (progressBar1!.Value < progressBar1.Maximum)
                progressBar1.Value++;
            else
            {
                timer1!.Stop();
                MessageBox.Show("Progress tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void Temizle()
        {
            txtId?.Clear();
            txtIsim?.Clear();
            cmbKonu!.SelectedIndex = 0;
            rtbMesaj?.Clear();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }
    }
}