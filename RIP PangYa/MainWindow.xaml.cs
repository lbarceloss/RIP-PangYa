using Calculadora;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Memory;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Net.NetworkInformation;
using System.Windows.Documents;

namespace RIP_PangYa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mem m = new Mem();
        private BackgroundWorker BGW = new BackgroundWorker();

        bool Aberto = false, isEditing = false;

        #region Variaveis
        // Variáveis para aba Game
        int altura, largura;
        int recoveryX2;
        float bolaDiametro, bolaMassa, quiqueBola;
        double gravidade, fatorForca, vetorQueda;
        double constanteCurva, constanteSpin;
        double matrizAlgo, naoSei1, naoSei2;
        // Variáveis para a aba Vento
        float cos, sen, ventoOculto;
        string vento;
        // Variáveis para a aba Hole e Bola
        float tee1, tee2, tee3, cosBola, senBola, bolaX, bolaY, spin, curva;
        float pin1, pin2, pin3, alturaHole;
        int terreno;
        // Variáveis para a aba Personagem
        float gridPersonagem, pangyaPixel, calibrador;
        byte[] semprePangYa;
        int barraEstado, powerShot;
        // Variáveis para a aba Memorial
        int memorial, efeitoItem, efeitoRaridade, itemGanho;
        // Variáveis para a aba Caldeirão
        int item1, item2, item3, item4, desconhecido1, desconhecido2, quantidadeMax, quantidadeFazer;
        int item1Green, item2Green, item3Green, item4Green, create, minimo, maximo, mais1, menos1;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            BGW.DoWork += BGW_DoWork;
            BGW.RunWorkerCompleted += BGW_RunWorkerCompleted;
            BGW.WorkerReportsProgress = true;
            BGW.WorkerSupportsCancellation = true;
        }

        private void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BGW.RunWorkerAsync();

            if (!isEditing) // Só atualiza se o usuário não estiver editando
            {
                Memories();
                // GAME
                AlturaTEXT.Text = altura.ToString();
                LarguraTEXT.Text = largura.ToString();
                RecoveryX2TEXT.Text = recoveryX2.ToString();
                GravityTEXT.Text = gravidade.ToString();
                FactorForcaTEXT.Text = fatorForca.ToString();
                BolaDiametroTEXT.Text = bolaDiametro.ToString();
                BolaMassaTEXT.Text = bolaMassa.ToString();
                ConstanteCurvaTEXT.Text = constanteCurva.ToString();
                ConstanteSpinTEXT.Text = constanteSpin.ToString();
                MatrizAlgoTEXT.Text = matrizAlgo.ToString();
                QuiqueBolaTEXT.Text = quiqueBola.ToString();
                VetorQuedaTEXT.Text = vetorQueda.ToString();
                NaoSei1TEXT.Text = naoSei1.ToString();
                NaoSei2TEXT.Text = naoSei2.ToString();
                // VENTO
                CosTEXT.Text = cos.ToString();
                SenTEXT.Text = sen.ToString();
                VentoTEXT.Text = vento.ToString();
                VentoOcultoTEXT.Text = ventoOculto.ToString();
                // BOLA E HOLE
                Tee1TEXT.Text = tee1.ToString();
                Tee2TEXT.Text = tee2.ToString();
                Tee3TEXT.Text = tee3.ToString();
                CosBolaTEXT.Text = cosBola.ToString();
                SenBolaTEXT.Text = senBola.ToString();
                BolaXTEXT.Text = bolaX.ToString();
                BolaYTEXT.Text = bolaY.ToString();
                SpinTEXT.Text = spin.ToString();
                CurvaTEXT.Text = curva.ToString();
                TerrenoTEXT.Text = terreno.ToString();
                Pin1TEXT.Text = pin1.ToString();
                Pin2TEXT.Text = pin2.ToString();
                Pin3TEXT.Text = pin3.ToString();
                AlturaHoleTEXT.Text = alturaHole.ToString();
                // PERSONAGEM
                GridPersonagemTEXT.Text = gridPersonagem.ToString();
                PangyaPixelTEXT.Text = pangyaPixel.ToString();
                //SemprePangYaTEXT.Text = semprePangYa.ToString();
                SemprePangYaTEXT.Text = BitConverter.ToString(semprePangYa).Replace("-", " ");
                CalibradorTEXT.Text = calibrador.ToString();
                BarraEstadoTEXT.Text = barraEstado.ToString();
                PowerShotTEXT.Text = powerShot.ToString();
                // MEMORIAL
                MemorialTEXT.Text = memorial.ToString();
                EfeitoItemTEXT.Text = efeitoItem.ToString();
                EfeitoRaridadeTEXT.Text = efeitoRaridade.ToString();
                ItemGanhoTEXT.Text = itemGanho.ToString();
                // CALDEIRAO
                Item1TEXT.Text = item1.ToString();
                Item2TEXT.Text = item2.ToString();
                Item3TEXT.Text = item3.ToString();
                Item4TEXT.Text = item4.ToString();
                Desconhecido1TEXT.Text = desconhecido1.ToString();
                Desconhecido2TEXT.Text = desconhecido2.ToString();
                QuantidadeMaxTEXT.Text = quantidadeMax.ToString();
                QuantidadeFazerTEXT.Text = quantidadeFazer.ToString();
                Item1GreenTEXT.Text = item1Green.ToString();
                Item2GreenTEXT.Text = item2Green.ToString();
                Item3GreenTEXT.Text = item3Green.ToString();
                Item4GreenTEXT.Text = item4Green.ToString();
                CreateTEXT.Text = create.ToString();
                MinimoTEXT.Text = minimo.ToString();
                MaximoTEXT.Text = maximo.ToString();
                Mais1TEXT.Text = mais1.ToString();
                Menos1TEXT.Text = menos1.ToString();
            } 
        }

        private void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            Aberto = m.OpenProcess("ProjectG");
            if (!Aberto)
            {
                Thread.Sleep(1000);
                return;
            }

            Thread.Sleep(100);
            BGW.ReportProgress(0);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #region Calculadora
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calc();
        }
        private double CheckValidInput(string value)
        {

            if (string.IsNullOrEmpty(value) || CheckDoubleInputValue(value) == false)
                return 0.0;

            return double.Parse(value);
        }
        private bool CheckDoubleInputValue(string input)
        {
            bool is_an_valid_number = true;

            try
            {
                double.Parse(input);
            }
            catch
            {
                is_an_valid_number = false;
            }
            return is_an_valid_number;
        }

        private object CheckValidInputSlope(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0.0;

            if (!double.TryParse(value, out double result))
            {
                string[] split = value.Split(',');
                if (split.Length != 3 || !double.TryParse(split[0], out _) || !double.TryParse(split[1], out _) || !double.TryParse(split[2], out _))
                    return 0.0;

                return new Vector3D(
                    double.Parse(split[0]) * Constraints.slope_break_to_curve_slope,
                    double.Parse(split[1]) * Math.PI / 180,
                    double.Parse(split[2]) * Constraints.slope_break_to_curve_slope
                );
            }
            return result;
        }

        private double DesvioByDegree(double yards, double distance)
        {
            return Math.Sin(Math.Atan2(yards * -1.5, distance)) * distance / 1.5;
        }

        private void Calc()
        {
            double power = CheckValidInput(textBox_charStats_power.Text);
            double auxpart_pwr = CheckValidInput(textBox_charStats_ring_power.Text);
            double card_pwr = CheckValidInput(textBox_charStats_card_power.Text);
            double mascot_pwr = CheckValidInput(textBox_charStats_mascot_power.Text);
            double card_ps_pwr = CheckValidInput(textBox_charStats_cardPower_ShotPower.Text);

            string _club = comboBox_Club.Text;


            if (_club.Contains("I") || _club.Contains("1") || _club.Contains("2") || _club.Contains("3"))
                _club = "_" + _club;
            ClubInfo club_info = ClubInfoData.CLUB_INFO[_club];

            int _shot = comboBox_Shot.SelectedIndex;
            string shot = ShotType.shot_type_enum[_shot];
            _shot = PowerShotFactory.GetIntValueFromShotTypePassingAString(shot);

            int pwshot = comboBox_PowerShot.SelectedIndex; // 0 - NO_POWER_SHOT, 1 - ONE_POWER_SHOT, 2 - TWO_POWER_SHOT, 3 - ITEM_15_POWER_SHOT
            int power_shot = PowerShotFactory.GetPowerShotFactory(pwshot);

            double distance = CheckValidInput(textBox_common_distance.Text);
            double height = CheckValidInput(textBox_common_height.Text);
            double wind = CheckValidInput(textBox_common_wind.Text);
            double degree = CheckValidInput(textBox_common_degree.Text);
            double ground = CheckValidInput(textBox_common_ground.Text);
            double spin = CheckValidInput(textBox_common_spin.Text);
            double curve = CheckValidInput(textBox_common_curve.Text);
            object slope_break = CheckValidInputSlope(textBox_common_slope_break.Text);

            if (ground == 0.0)
                ground = 100.0;

            Wind _wind = new Wind();
            _wind.wind = wind;

            Options power_player_options = new Options(
                distance,
                1.0,
                ground,
                0.0,
                0.0,
                spin,
                curve,
                null,
                _shot,
                (int)pwshot, //power_shot,
                new Power((int)power, new PowerOptions(
                    (int)auxpart_pwr, (int)mascot_pwr, (int)card_pwr, 0, 0, (int)card_ps_pwr
                ))
             );

            Dictionary<string, object> found = Utils.FindPower(
                power_player_options,
                club_info,
                _shot,
                (int)pwshot, //(int)power_shot,
                distance,
                height,
                _wind,
                degree,
                ground,
                spin,
                curve,
                slope_break,
                0,
                1.0
            );


            List<Dictionary<string, object>> f = new List<Dictionary<string, object>> { found };
            int index_f = 0;
            if (Convert.ToInt32(found["power"]) != -1)
            {
                do
                {
                    index_f++;
                    f.Add(
                        Utils.FindPower(
                            power_player_options,// power_player
                            club_info,
                            _shot,
                            power_shot,
                            distance,
                            height,
                            _wind,
                            degree,
                            ground,
                            spin,
                            curve,
                            slope_break,
                            Math.Atan2(Convert.ToDouble(f[index_f - 1]["desvio"]) * 1.5, distance),
                            Convert.ToDouble(f[index_f - 1]["power"])
                        )
                    );

                } while (Convert.ToDouble(f[index_f]["power"]) != -1 && Convert.ToDouble(f[index_f - 1]["power"]) != -1 && Math.Abs(Convert.ToDouble(f[index_f - 1]["desvio"]) - Convert.ToDouble(f[index_f]["desvio"])) >= 0.05);
            }

            if (Convert.ToDouble(f[index_f]["power"]) != -1)
            {
                label_result.Foreground = Brushes.Black;
                label_desviopb.Foreground = Brushes.Black;
                label_calippers8.Foreground = Brushes.Black;

                label_result.Content = Math.Round((Convert.ToDouble(f[index_f]["power"]) * 100), 2, MidpointRounding.ToEven).ToString();

                label_desviopb.Content = Math.Round((DesvioByDegree(Convert.ToDouble(f[index_f]["desvio"]), distance) / 0.2167), 2, MidpointRounding.ToEven).ToString();

                label_calippers8.Content = Math.Round((Convert.ToDouble(f[index_f]["power_range"]) * Convert.ToDouble(f[index_f]["power"])), 1, MidpointRounding.ToEven).ToString() + "y";

            }
            else
            {
                label_result.Foreground = Brushes.DarkRed;
                label_desviopb.Foreground = Brushes.DarkRed;
                label_calippers8.Foreground = Brushes.DarkRed;

                label_result.Content = "######";
                label_desviopb.Content = "######";
                label_calippers8.Content = "######";
            }
        }
        #endregion
        #region Memories
        private void Memories()
        {
            // ABA GAME
            altura = m.ReadInt("ProjectG.exe+B02330", "");
            largura = m.ReadInt("ProjectG.exe+B02334", "");
            recoveryX2 = m.ReadInt("ProjectG.exe+0117B5C0, 0x498", "");
            gravidade = m.ReadDouble("ProjectG.exe+90AA28", "", false);
            fatorForca = m.ReadDouble("00D66CB8", "", false);
            bolaDiametro = m.ReadFloat("ProjectG.exe+A47EF8", "", false);
            bolaMassa = m.ReadFloat("00E47EF4", "", false);
            constanteCurva = m.ReadDouble("00D17908", "", false);
            constanteSpin = m.ReadDouble("00D19B98", "", false);
            matrizAlgo = m.ReadDouble("00D00190", "", false);
            quiqueBola = m.ReadFloat("00EFD134", "", false);
            vetorQueda = m.ReadDouble("00E42544", "", false);
            naoSei1 = m.ReadDouble("00D16758", "", false);
            naoSei2 = m.ReadDouble("00D083A0", "", false);
            //ABA VENTO
            cos = m.ReadFloat("ProjectG.exe+00A73E60, 0x34, 0x18, 0x10, 0x30, 0x0, 0x234, 0xAC", "", false);
            sen = m.ReadFloat("ProjectG.exe+00A73E60, 0x34, 0x18, 0x10, 0x30, 0x0, 0x234, 0xB4", "", false);
            vento = m.ReadString("ProjectG.exe+00B006E8, 0x34, 0x18, 0x10, 0x30, 0x0, 0x234, 0xB4", "", 2);
            ventoOculto = m.ReadFloat("00E79074", "", false);
            // BOLA E HOLE
            tee1 = m.ReadFloat("ProjectG.exe+A47E30", "", false);
            tee2 = m.ReadFloat("ProjectG.exe+A47F00", "", false);
            tee3 = m.ReadFloat("ProjectG.exe+A47F04", "", false);
            cosBola = m.ReadFloat("ProjectG.exe+B024A0", "", false);
            senBola = m.ReadFloat("ProjectG.exe+B024A8", "", false);
            bolaX = m.ReadFloat("ProjectG.exe+00A73E60, 0x34, 0x18, 0x10, 0x30, 0x0, 0x21C, 0x1C", "", false);
            bolaY = m.ReadFloat("ProjectG.exe+00B006E8, 0x8, 0x10, 0x30, 0x0, 0x21C, 0x24", "", false);
            spin = m.ReadFloat("ProjectG.exe+00A73E60, 0x34, 0x18, 0x10, 0x3C, 0x30, 0x0, 0x1C", "", false);
            curva = m.ReadFloat("ProjectG.exe+00B006E8, 0x8, 0xC, 0xC, 0x40, 0x0, 0x0, 0x18", "", false);
            terreno = m.ReadInt("ProjectG.exe+00B006E8, 0x8, 0xC, 0xC, 0x30, 0x0, 0x21C, 0xAC", "");
            pin1 = m.ReadFloat("ProjectG.exe+AFD154", "", false);
            pin2 = m.ReadFloat("ProjectG.exe+AFD158", "", false);
            pin3 = m.ReadFloat("ProjectG.exe+AFD15C", "", false);
            alturaHole = m.ReadFloat("ProjectG.exe+00B006E8, 0x8, 0x10, 0x30, 0x0, 0x218, 0x0, 0x34", "", false);
            // PERSONAGEM
            gridPersonagem = m.ReadFloat("ProjectG.exe+00A73E60, 0x34, 0xB4, 0x28, 0x14, 0x30, 0x0, 0x7C", "", false);
            pangyaPixel = m.ReadFloat("ProjectG.exe+00B006E8, 0x20, 0x10, 0xB4, 0x30, 0x8, 0x128, 0xE8", "", true);
            semprePangYa = m.ReadBytes("ProjectG.exe+29B192", 5, "");
            calibrador = m.ReadFloat("ProjectG.exe+00B006E8, 0x20, 0x10, 0xB4, 0x30, 0x8, 0x128, 0x100", "", false);
            barraEstado = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x10, 0xB4, 0x30, 0x8, 0x128, 0xF8", "");
            // MEMORIAL
            memorial = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x588");
            efeitoItem = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x520");
            efeitoRaridade = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x58C");
            itemGanho = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x18");
            // CALDEIRAO
            item1 = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x3E0");
            item2 = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x450");
            item3 = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x4C0");
            item4 = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x530");
            desconhecido1 = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x670");
            desconhecido2 = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x688");
            quantidadeMax = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x59C");
            quantidadeFazer = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x598");
            item1Green = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x3E0");
            item2Green = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x3E0");
            item3Green = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x3E0");
            item4Green = m.ReadInt("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, 0x3E0");
            create = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x20, 0x130, 0x44");
            minimo = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x164, 0x24, 0x10C, 0x14C, 0x134, 0x44");
            maximo = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x164, 0x24, 0x10C, 0x24, 0x140, 0x44");
            mais1 = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x164, 0x24, 0x10C, 0x24, 0x138, 0x44");
            menos1 = m.ReadByte("ProjectG.exe+00B006E8, 0x20, 0x164, 0x24, 0x10C, 0x20, 0x140, 0x44");
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BGW.RunWorkerAsync();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isEditing = true;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    string memoryAddress = "";
                    string dataType = "";

                    // Verifica qual tipo de dado deve ser escrito
                    if (textBox.Name == "AlturaTEXT" || textBox.Name == "LarguraTEXT" ||
                        textBox.Name == "RecoveryX2TEXT" || textBox.Name == "TerrenoTEXT" ||
                        textBox.Name == "MemorialTEXT" || textBox.Name == "EfeitoItemTEXT" ||
                        textBox.Name == "EfeitoRaridadeTEXT" || textBox.Name == "ItemGanhoTEXT" ||
                        textBox.Name.Contains("Item") || textBox.Name.Contains("Quantidade") ||
                        textBox.Name.Contains("Create") || textBox.Name.Contains("Minimo") ||
                        textBox.Name.Contains("Maximo") || textBox.Name.Contains("Mais") ||
                        textBox.Name.Contains("Menos"))
                    {
                        dataType = "int";
                    }
                    else if (textBox.Name == "SemprePangYaTEXT" ||
                             textBox.Name == "CreateTEXT" || textBox.Name.Contains("Desconhecido"))
                    {
                        dataType = "byte";
                    }
                    else if (textBox.Name == "GravityTEXT" || textBox.Name == "FactorForcaTEXT" ||
                             textBox.Name == "ConstanteCurvaTEXT" || textBox.Name == "ConstanteSpinTEXT" ||
                             textBox.Name == "MatrizAlgoTEXT" || textBox.Name == "VetorQuedaTEXT" ||
                             textBox.Name == "NaoSei1TEXT" || textBox.Name == "NaoSei2TEXT")
                    {
                        dataType = "double";
                    }
                    else
                    {
                        dataType = "float"; // Todas as outras variáveis são float
                    }

                    // Mapeia o endereço da memória para cada TextBox
                    switch (textBox.Name)
                    {
                        case "AlturaTEXT": memoryAddress = "ProjectG.exe+B02330"; break;
                        case "LarguraTEXT": memoryAddress = "ProjectG.exe+B02334"; break;
                        case "RecoveryX2TEXT": memoryAddress = "ProjectG.exe+0117B5C0, 0x498"; break;
                        case "GravityTEXT": memoryAddress = "ProjectG.exe+90AA28"; break;
                        case "FactorForcaTEXT": memoryAddress = "00D66CB8"; break;
                        case "BolaDiametroTEXT": memoryAddress = "ProjectG.exe+A47EF8"; break;
                        case "BolaMassaTEXT": memoryAddress = "00E47EF4"; break;
                        case "ConstanteCurvaTEXT": memoryAddress = "00D17908"; break;
                        case "ConstanteSpinTEXT": memoryAddress = "00D19B98"; break;
                        case "MatrizAlgoTEXT": memoryAddress = "00D00190"; break;
                        case "VetorQuedaTEXT": memoryAddress = "00E42544"; break;
                        case "NaoSei1TEXT": memoryAddress = "00D16758"; break;
                        case "NaoSei2TEXT": memoryAddress = "00D083A0"; break;
                        case "SemprePangYaTEXT": memoryAddress = "ProjectG.exe+29B192"; break;
                        case "MemorialTEXT": memoryAddress = "ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x588"; break;
                        case "EfeitoItemTEXT": memoryAddress = "ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x520"; break;
                        case "EfeitoRaridadeTEXT": memoryAddress = "ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x58C"; break;
                        case "ItemGanhoTEXT": memoryAddress = "ProjectG.exe+00B006E8, 0x20, 0x194, 0x28, 0x0, 0x8, 0x24, 0x18"; break;
                        default:
                            if (textBox.Name.Contains("Item") || textBox.Name.Contains("Quantidade") ||
                                textBox.Name.Contains("Create") || textBox.Name.Contains("Minimo") ||
                                textBox.Name.Contains("Maximo") || textBox.Name.Contains("Mais") ||
                                textBox.Name.Contains("Menos"))
                            {
                                memoryAddress = "ProjectG.exe+00B006E8, 0x20, 0x164, 0x134, 0x24, 0x10C, 0x20, " +
                                                (textBox.Name.Contains("1") ? "3E0" :
                                                 textBox.Name.Contains("2") ? "450" :
                                                 textBox.Name.Contains("3") ? "4C0" :
                                                 textBox.Name.Contains("4") ? "530" :
                                                 textBox.Name.Contains("Max") ? "59C" :
                                                 textBox.Name.Contains("Fazer") ? "598" : "000");
                            }
                            break;
                    }

                    if (dataType == "int" && int.TryParse(textBox.Text, out int intValue))
                    {
                        //Console.WriteLine($"Escrevendo {intValue} em {memoryAddress}");
                        m.WriteMemory(memoryAddress, "int", intValue.ToString());
                    }
                    else if (dataType == "float" && float.TryParse(textBox.Text, out float floatValue))
                    {
                        //Console.WriteLine($"Escrevendo {floatValue} em {memoryAddress}");
                        m.WriteMemory(memoryAddress, "float", floatValue.ToString());
                    }
                    else if (dataType == "double" && double.TryParse(textBox.Text, out double doubleValue))
                    {
                        //Console.WriteLine($"Escrevendo {doubleValue} em {memoryAddress}");
                        m.WriteMemory(memoryAddress, "double", doubleValue.ToString());
                    }
                    else if (dataType == "byte" && byte.TryParse(textBox.Text, out byte byteValue))
                    {
                        //Console.WriteLine($"Escrevendo {byteValue} em {memoryAddress}");
                        m.WriteMemory(memoryAddress, "byte", byteValue.ToString());
                    }
                }

                isEditing = false;
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

    }
}
