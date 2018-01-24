using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Projekt3_GrzegrzółkaMateusz46792
{
    public partial class mgAnalizator_funkcji_Fx : Form
    {
        public mgAnalizator_funkcji_Fx()
        {
            InitializeComponent();
            mgcmbWybranyStylLinii.SelectedIndex = 4;
        }

        private void mgcmbWybranyStylLinii_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* odczytanie wybranego stylu linii i przypisanie go atrybutowi BorderDashStyle serii danych kontrolki mgWykres*/
            mgchWykres.Series[0].BorderDashStyle = mgWybranyStylLinii();
            ChartDashStyle mgWybranyStylLinii()
            {
                switch (mgcmbWybranyStylLinii.SelectedIndex)
                {
                    case 0: return ChartDashStyle.Dash;
                    case 1: return ChartDashStyle.DashDot;
                    case 2: return ChartDashStyle.DashDotDot;
                    case 3: return ChartDashStyle.Dot;
                    case 4: return ChartDashStyle.Solid;
                    default: return ChartDashStyle.Solid;
                }
            }

        }
        float mgObliczenieFx(float mgX, float mgEps)
        {
            //deklarujemy pustą zmienną na przechowywanie sumy
            float mgSuma = 0.0f;
            //bo wiemy że w pierwszym kroku 1
            int mgi = 2;

            float mgWyraz = 1.0f;

            //idziemy pętlą do while od 1
            do
            {
                // "przejście" do kolejnego wyrazu szeregu
                mgi++;

                // "przejście" do kolejnego wyrazu szeregu


                // obliczenie i-tego wyrazu szeregu
                mgWyraz *= (mgX * (float)Math.Log(mgi)) * (1 / (mgi * (float)Math.Log(mgi - 1)));
                // obliczenie kolejnego przybliżenia sumy szeregu
                mgSuma = mgSuma + mgWyraz;

            }
            while (Math.Abs(mgWyraz) > mgEps);

            //korekta ze względu na znalezienie dokładności w pierwszym kroku pętli
            if (mgSuma == 0 && mgWyraz > 0)
            {
                mgSuma = mgWyraz;
            }
            return mgSuma;
        }

        private void mgbtnObliczWartośćFunkcji_Click(object sender, EventArgs e)
        {
            /* deklaracje zmiennych dla przechowania danych wejściowych pobranych z formularza */
            float mgEps, mgX;
            //pobranie danych wejściowych : X oraz Eps
            //sprawdzenie, czy została wpisana wartość zmiennej X
            if (String.IsNullOrEmpty(mgtxtX.Text))
            {
                //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtX, "BŁĄD: musisz podać wartość zmiennej niezależnej X");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK

            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            //pobranie wartości zmiennej X (ze sprawdzeniem poprawności zapisu)
            if (!float.TryParse(mgtxtX.Text, out mgX))
            { //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtX, "BŁĄD: wystąpił niedozwolony znak w zapisie wartości X");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            /*Ponieważ szereg jest zbieżny dla wszystkich wartości X, nie sprawdzamy
                                             czy X należy do określonego przedziału zbieżności szeregu*/
            //pobranie dokładności obliczeń Eps
            //sprawdzenie, czy została wpisana dokładność obliczeń Eps
            if (String.IsNullOrEmpty(mgtxtEps.Text))
            {
                //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtEps, "BŁĄD: musisz podać dokładność obliczeń Eps!");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK

            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            //pobranie dokładości obliczeń Eps (ze sprawdzeniem poprawności zapisu)
            if (!float.TryParse(mgtxtEps.Text, out mgEps))
            {
                mgerrorProvider1.SetError(mgtxtEps, "BŁĄD: wystąpił niedozwolony znak w zapisie wartości Eps");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            //sprawdzenie warunku wejściowego dla dokładności obliczeń Eps
            if ((mgEps <= 0.0f) || (mgEps >= 1.0f))
            {
                mgerrorProvider1.SetError(mgtxtEps, "BŁĄD: dokładność obliczeń Eps musi spełniać warunek wejściowy: 0.0 < Eps <1.0");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            //deklaracja zmiennej dla przechowania obliczonej wartości funkcji F(x)
            float mgObliczonaWartośćFx;
            /*  obliczenie wartości funkcji F(X), czyli wyznaczenie sumy podanego szeregu funkcyjnego (z dokładnością Eps) */
            mgObliczonaWartośćFx = mgObliczenieFx(mgX, mgEps);

            //wizualizacja obliczonej wartości funkcji F(X)
            mgtxtWartośćFx.Text = Math.Round(mgObliczonaWartośćFx, 3, MidpointRounding.AwayFromZero).ToString();
            //odsłonięcie kontrolek z wynikiem obliczeń
            mglabWartośćFx.Visible = true;
            mgtxtWartośćFx.Visible = true;
            /*ustawienie stanu braku aktywności przycisku poleceń: Obliczenie wartości funkcji F(X)*/
            mgbtnObliczWartośćFunkcji.Enabled = false;
        }
        bool mgPobierzDaneWejściowe(out float mgEps, out float mgXd, out float mgXg, out float mgh)
        {
            /* ustawienie domyślnych wartości dla parametrów wyjściowych, gdy chcemy zapalać kontrolkę errorProvider1 dla sygnalizacji błędów*/
            mgEps = 0.0F; mgXd = 0.0F; mgXg = 0.0F; mgh = 0.0F;

            //pobranie dokładności obliczeń Eps
            //sprawdzenie, czy została wpisana dokładność obliczeń Eps
            if (string.IsNullOrEmpty(mgtxtEps.Text))
            { /*zapalenie kontrolki errorProvider (sygnalizacja błędu)*/
                mgerrorProvider1.SetError(mgtxtEps, "BŁĄD: musisz podać dokładność obliczeń Eps!");
                return false; /*zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            /*pobranie dokładności obliczeń Eps ( ze sprawdzeniem poprawności zapisu)*/
            if (!float.TryParse(mgtxtEps.Text, out mgEps))
            {
                mgerrorProvider1.SetError(mgtxtEps, "BŁĄD: wystąpił niedozwolony znak w zapisie wartości Eps");
                return false; /* zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            // sprawdzenie warunku wejściowego dla dokładności obliczeń Eps
            if ((mgEps <= 0.0F) || (mgEps >= 1.0F))
            {
                mgerrorProvider1.SetError(mgtxtEps, "BŁAD: dokładność obliczeń Eps musi spełniać warunek wejściowy: 0.0 < Eps < 1.0");
                return false; /* zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            // pobranie dolnej granicy przedziału wartości zmiennej X
            // sprawdzenie, czy została wpisana dolna granica przedziału wartości zmiennej X
            if (string.IsNullOrEmpty(mgtxtXd.Text))
            {/*zapalenie kontrolki errorProvider (sygnalizacja błędu)*/
                mgerrorProvider1.SetError(mgtxtXd, "BŁĄD: musisz podać wartość Xd (dolnej granicy przedziału wartości X");
                return false; /*zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1

            /*pobranie wartości Xd (ze sprawdzeniem poprawności zapisu)*/
            if (!float.TryParse(mgtxtXd.Text, out mgXd))
            {
                mgerrorProvider1.SetError(mgtxtXd, "BŁĄD: wystąpił niedozwolony znak w zapisie wartości Xd");
                return false; /* zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1

            // pobranie górnej granicy przedziału wartości zmiennej X
            // sprawdzenie, czy została wpisana górna granica przedziału wartości zmiennej X
            if (string.IsNullOrEmpty(mgtxtXg.Text))
            {/*zapalenie kontrolki errorProvider (sygnalizacja błędu)*/
                mgerrorProvider1.SetError(mgtxtXg, "BŁĄD: musisz podać wartość Xg (górnej granicy przedziału wartości X");
                return false; /*zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1

            /*pobranie wartości Xg (ze sprawdzeniem poprawności zapisu)*/
            if (!float.TryParse(mgtxtXg.Text, out mgXg))
            {
                mgerrorProvider1.SetError(mgtxtXg, "BŁĄD: wystąpił niedozwolony znak w zapisie wartości Xg");
                return false; /* zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            // sprawdzenie warunku wejściowego dla granic przedziału wartości [Xd, Xg]
            if (mgXd > mgXg)
            {
                mgerrorProvider1.SetError(mgtxtXg, "BŁĄD: dolna granica przedziału wartości Xd nie może być większa od górnej granicy przedziału wartości Xg");
                return false; /* zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1

            //pobranie kroku h (odstępem pomiędzy kolejnymi punktami przedziału [Xd, Xg])
            //sprawdzenie, czy został wpisany przyrost h (krok zmian X)
            if (string.IsNullOrEmpty(mgtxtH.Text))
            {/*zapalenie kontrolki errorProvider (sygnalizacja błędu)*/
                mgerrorProvider1.SetError(mgtxtH, "BŁĄD: musisz podać wartość przyrostu h (kroku zmian X)");
                return false; /*zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1

            /* pobranie wartości przyrostu h (ze sprawdzeniem poprawności zapisu)*/
            if (!float.TryParse(mgtxtH.Text, out mgh))
            {
                mgerrorProvider1.SetError(mgtxtH, "BŁĄD: wystąpił niedozwolony znak w zapisie wartości h");
                return false; /* zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            // sprawdzenie warunku wejściowego dla dolnej granicy przedziału wartości h
            if ((mgh <= 0) || (mgh >= 1))
            {
                mgerrorProvider1.SetError(mgtxtH, "BŁĄD: przyrost h (krok zmian zmiennej X powinien spełniać warunek: 0 < h < 1");
                return false; /* zakończenie pobierania danych wejściowych i zwrotne przekazanie wartości "false" */
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1

            return true; /* nie było błędu przy pobieraniu danych wejściowych z formularza*/
        }
        void mgTablicowanieWartościFunkcji(ref float[,] mgTWF, float mgEps, float mgXd, float mgXg, float mgh)
        //gdzie TWF, to TabelaWartościFunkcji
        {
            //deklaracje pomocnicze
            float mgX; //zmienna niezależna X
            int mgi; //numer podprzedziału w przedziale
            for (mgX = mgXd, mgi = 0; mgX <= mgXg; mgi++, mgX = mgX + mgh)
            {
                mgTWF[mgi, 0] = mgX;
                mgTWF[mgi, 1] = mgObliczenieFx(mgX, mgEps);
            }
        }

        private void mgbtnTabelarycznaWizualizacjaFunkcji_Click(object sender, EventArgs e)
        {
            /*deklaracja zmiennych lokalnych dla przechowywania danych wejściowych, które będą pobrane z kontrolek formularza programu*/
            float mgEps, mgXd, mgXg, mgh;
            //pobranie danych wejściowych do obsługi zdarzenia CLICK
            if (!mgPobierzDaneWejściowe(out mgEps, out mgXd, out mgXg, out mgh))
                return; /*był wykryty błąd przy pobieraniu danych*/
            //wyznaczenie liczby  punktów Xi w przedziale [Xd,Xg]
            int mgn = (int)((mgXg - mgXd) / mgh) + 1;
            //utworzenie egzemplarza tabeli dla tablicowania wartości funkcji
            float[,] mgTabelaWartościFunkcji = new float[mgn, 2];
            mgTablicowanieWartościFunkcji(ref mgTabelaWartościFunkcji,(float)mgEps, mgXd, mgXg, mgh);
            //ukrycie grafiki powitalnej
            mgGrafikaPowitalna.Visible = false;
            //odsłonięcie kontrolki DataGridView
            mgdvgTabelaFx.Visible = true;
            //ukrycie wykresu
            mgchWykres.Visible = false;
            //ukrycie kontrolek z pojedyńczą wartością F(X)
            mglabWartośćFx.Visible = false;
            mgtxtWartośćFx.Visible = false;
            //wyzerowanie wierszy danych
            mgdvgTabelaFx.Rows.Clear();
            //wycentrowanie zapisu w kolumnach
            mgdvgTabelaFx.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            mgdvgTabelaFx.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //PRZEPISYWANIE DANYCH (najpierw nowy wiersz, potem dane)
            for (int mgi = 0; mgi < mgTabelaWartościFunkcji.GetLength(0); mgi++)
            {
                mgdvgTabelaFx.Rows.Add();//dodanie nowego wiersza
                //wpisanie wartości X
                mgdvgTabelaFx.Rows[mgi].Cells[0].Value = String.Format("{0:f2}", mgTabelaWartościFunkcji[mgi, 0]);
                //wpisanie wartości funkcji  F(X)
                mgdvgTabelaFx.Rows[mgi].Cells[1].Value = String.Format("{0:f2}", mgTabelaWartościFunkcji[mgi, 1]);
            }
            koloryWykresuToolStripMenuItem.Enabled = false;
            stylLiniiToolStripMenuItem.Enabled = false;
            grubośćLiniiToolStripMenuItem.Enabled = false;
            mgtypWykresuToolStripMenuItem.Enabled = false;
            stylCzcionkiToolStripMenuItem.Enabled = true;
            mgzapiszTabelęZmianWartościFunkcjiWPlikuToolStripMenuItem.Enabled = true;
            mgodczytajTabelęZmianWartościFunkcjiZPlikuToolStripMenuItem.Enabled = true;
            mgbtnTabelarycznaWizualizacjaFunkcji.Enabled = false;
        }

        private void mgbtnGraficznaWizualizacjaFunkcji_Click(object sender, EventArgs e)
        {
            /*deklaracja zmiennych lokalnych dla przechowywania danych wejściowych, które będą pobrane z kontrolek formularza programu*/
            float mgEps, mgXd, mgXg, mgh;
            if (!mgPobierzDaneWejściowe(out mgEps, out mgXd, out mgXg, out mgh))
                return; /*był wykryty błąd przy pobieraniu danych*/
            int mgn = (int)((mgXg - mgXd) / mgh) + 1;
            //utworzenie egzemplarza tabeli dla tablicowania wartości funkcji
            float[,] mgTabelaWartościFunkcji = new float[mgn, 2];
            mgTablicowanieWartościFunkcji(ref mgTabelaWartościFunkcji, mgEps, mgXd, mgXg, mgh);
            mgchWykres.Visible = true;
            mgGrafikaPowitalna.Visible = false;
            mgdvgTabelaFx.Visible = false;
            mglabWartośćFx.Visible = false;
            mgtxtWartośćFx.Visible = false;
            //lokalizacja i wymiarowanie wykresu
            mgchWykres.Location = new Point(230, 120);
            mgchWykres.Width = (int)(this.Width * 0.45f);
            mgchWykres.Height = (int)(this.Height * 0.5f);
            //tytuł wykresu
            mgchWykres.Titles.Add("Wykres zmian wartości funkcji");
            //umieszczenie legendy pod wykresem
            mgchWykres.Legends.FindByName("Legend1").Docking = Docking.Bottom;
            mgchWykres.BackColor = Color.Beige;
            //ustalenie marginesów między powierzchnią wykresu a krawędziami kontrolki chart
            mgchWykres.ChartAreas[0].Position.X = 10;
            mgchWykres.ChartAreas[0].Position.Y = 10;
            mgchWykres.ChartAreas[0].Position.Width = 80; //80%
            mgchWykres.ChartAreas[0].Position.Height = 80;
            mgchWykres.ChartAreas[0].AxisX.Title = "Wartości X";
            mgchWykres.ChartAreas[0].AxisY.Title = "Wartości F(X)";
            //formatowanie opisów
            mgchWykres.ChartAreas[0].AxisX.LabelStyle.Format = "{0:f2}";
            mgchWykres.ChartAreas[0].AxisY.LabelStyle.Format = "{0:f2}";
            //"wyzerowanie" serii danych
            mgchWykres.Series.Clear();
            mgchWykres.Series.Add("Seria 0");
            //formatowanie linii
            mgchWykres.Series[0].XValueMember = "X";
            mgchWykres.Series[0].YValueMembers = "Y";
            mgchWykres.Series[0].IsVisibleInLegend = true;
            mgchWykres.Series[0].Name = "Wartość funkcji F(X)";
            mgchWykres.Series[0].ChartType = SeriesChartType.Line;
            mgchWykres.Series[0].Color = Color.Black; //dla linii
            mgchWykres.Series[0].BorderDashStyle = ChartDashStyle.Solid;
            mgchWykres.Series[0].BorderWidth = 1;
            //dodawanie danych
            for (int mgi = 0; mgi < mgTabelaWartościFunkcji.GetLength(0); mgi++)
            {
                mgchWykres.Series[0].Points.AddXY(mgTabelaWartościFunkcji[mgi, 0], mgTabelaWartościFunkcji[mgi, 1]);
            }
            stylCzcionkiToolStripMenuItem.Enabled = false;
            mgzapiszTabelęZmianWartościFunkcjiWPlikuToolStripMenuItem.Enabled = false;
            mgodczytajTabelęZmianWartościFunkcjiZPlikuToolStripMenuItem.Enabled = false;
            koloryWykresuToolStripMenuItem.Enabled = true;
            stylLiniiToolStripMenuItem.Enabled = true;
            grubośćLiniiToolStripMenuItem.Enabled = true;
            mgtypWykresuToolStripMenuItem.Enabled = true;
            mgbtnGraficznaWizualizacjaFunkcji.Enabled = false;
        }

        private void mgbtnWybierzKolorLinii_Click(object sender, EventArgs e)
        {
            ColorDialog mgOknoKolorów = new ColorDialog();
            mgOknoKolorów.Color = mgchWykres.Series[0].Color;
            if (mgOknoKolorów.ShowDialog() == DialogResult.OK)
            {
                //ustawienie nowego koloru w wzierniku koloru linii
                mgtxtWybranyKolorLinii.BackColor = mgOknoKolorów.Color;
                //ustawienie nowego koloru linii wykresu
                mgchWykres.Series[0].Color = mgOknoKolorów.Color;
            }
        }

        private void mgbtnWybranieKoloruTła_Click(object sender, EventArgs e)
        {
            ColorDialog mgOknoKolorów = new ColorDialog();
            mgOknoKolorów.Color = mgchWykres.BackColor;
            /*wyświetlamy okno dialogowe dla wyboru kolorów i sprawdzamy czy kolor został wybrany 
             (czy użytkownik kliknął przycisk OK)*/
            if (mgOknoKolorów.ShowDialog() == DialogResult.OK)
            {
                //ustawienie nowego koloru w wzierniku koloru tła
                mgtxtWybranyKolorTła.BackColor = mgOknoKolorów.Color;
                //ustawienie nowego koloru tła wykresu
                mgchWykres.BackColor = mgOknoKolorów.Color;
            }

        }

        private void mgtrackBarGrubośćLinii_Scroll(object sender, EventArgs e)
        {
          

            mgtxtGrubośćLinii.Text = mgtrackBarGrubośćLinii.Value.ToString();
            mgchWykres.Series[0].BorderWidth = mgtrackBarGrubośćLinii.Value;
        }

        private void mgtxtGrubośćLinii_TextChanged(object sender, EventArgs e)
        {
            int mggr;
            if (!int.TryParse(mgtxtGrubośćLinii.Text, out mggr))
            {

                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            }
            if ((mggr > 10)|| (mggr < 1))
            {
                mgerrorProvider1.SetError(mgtxtGrubośćLinii, "BŁAD: Grubość linii musi być podana w zakresie od 1 do 10");
                return;
            }
            else
            {
                mgtrackBarGrubośćLinii.Value = int.Parse(mgtxtGrubośćLinii.Text);

                mgchWykres.Series[0].BorderWidth = mgtrackBarGrubośćLinii.Value;
            }
        }

        private void mgrdbOsieUkładuBezOpisu_CheckedChanged(object sender, EventArgs e)
        {
            mgchWykres.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            mgchWykres.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            mgchWykres.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.None;
            mgchWykres.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.None;
        }

        private void mgrdbOsieUkładuZopisem_CheckedChanged(object sender, EventArgs e)
        {
            mgchWykres.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
            mgchWykres.ChartAreas[0].AxisY.LabelStyle.Enabled = true;
            mgchWykres.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            mgchWykres.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;
        }

        private void mgplikToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mgliniowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].ChartType = SeriesChartType.Line;
            mgcmbWybranyStylLinii.Enabled = false;
            stylLiniiToolStripMenuItem.Enabled = false;
        }

        private void kolumnowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].ChartType = SeriesChartType.Column;
            mgcmbWybranyStylLinii.Enabled = false;
            stylLiniiToolStripMenuItem.Enabled = false;
        }

        private void słupkowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].ChartType = SeriesChartType.Bar;
            mgcmbWybranyStylLinii.Enabled = false;
            stylLiniiToolStripMenuItem.Enabled = false;
        }

        private void mgpunktowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].ChartType = SeriesChartType.Point;
            mgcmbWybranyStylLinii.Enabled = false;
            stylLiniiToolStripMenuItem.Enabled = false;
        }

        private void krójPismaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* utworzenie instancji (egzemplarza) klasy FontDialog, która umożliwia formatowanie czcionki*/
            FontDialog mgOknoFormatowaniaCzcionki = new FontDialog();
            /*wyświetlenie okna dialogowego formatowania czcionki i przypisanie kontrolce DataGridView tych ustawień*/
            if (mgOknoFormatowaniaCzcionki.ShowDialog() == DialogResult.OK)
                mgdvgTabelaFx.Font = mgOknoFormatowaniaCzcionki.Font;
        }

        private void rozmiarCzcionkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* utworzenie instancji (egzemplarza) klasy FontDialog, która umożliwia formatowanie czcionki*/
            FontDialog mgOknoFormatowaniaCzcionki = new FontDialog();
            /*wyświetlenie okna dialogowego formatowania czcionki i przypisanie kontrolce DataGridView tych ustawień*/
            if (mgOknoFormatowaniaCzcionki.ShowDialog() == DialogResult.OK)
                mgdvgTabelaFx.Font = mgOknoFormatowaniaCzcionki.Font;
        }

        private void pogrubionaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* utworzenie instancji (egzemplarza) klasy FontDialog, która umożliwia formatowanie czcionki*/
            FontDialog mgOknoFormatowaniaCzcionki = new FontDialog();
            /*wyświetlenie okna dialogowego formatowania czcionki i przypisanie kontrolce DataGridView tych ustawień*/
            if (mgOknoFormatowaniaCzcionki.ShowDialog() == DialogResult.OK)
                mgdvgTabelaFx.Font = mgOknoFormatowaniaCzcionki.Font;
        }

        private void kursywaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* utworzenie instancji (egzemplarza) klasy FontDialog, która umożliwia formatowanie czcionki*/
            FontDialog mgOknoFormatowaniaCzcionki = new FontDialog();
            /*wyświetlenie okna dialogowego formatowania czcionki i przypisanie kontrolce DataGridView tych ustawień*/
            if (mgOknoFormatowaniaCzcionki.ShowDialog() == DialogResult.OK)
                mgdvgTabelaFx.Font = mgOknoFormatowaniaCzcionki.Font;
        }

        private void pogrubionaIKursywaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* utworzenie instancji (egzemplarza) klasy FontDialog, która umożliwia formatowanie czcionki*/
            FontDialog mgOknoFormatowaniaCzcionki = new FontDialog();
            /*wyświetlenie okna dialogowego formatowania czcionki i przypisanie kontrolce DataGridView tych ustawień*/
            if (mgOknoFormatowaniaCzcionki.ShowDialog() == DialogResult.OK)
                mgdvgTabelaFx.Font = mgOknoFormatowaniaCzcionki.Font;
        }

        private void mgkolortłaWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog mgOknoKolorów = new ColorDialog();
            mgOknoKolorów.Color = mgchWykres.BackColor;
            /*wyświetlamy okno dialogowe dla wyboru kolorów i sprawdzamy czy kolor został wybrany 
             (czy użytkownik kliknął przycisk OK)*/
            if (mgOknoKolorów.ShowDialog() == DialogResult.OK)
            {
                //ustawienie nowego koloru w wzierniku koloru tła
                mgtxtWybranyKolorTła.BackColor = mgOknoKolorów.Color;
                //ustawienie nowego koloru tła wykresu
                mgchWykres.BackColor = mgOknoKolorów.Color;
            }
        }

        private void koloryWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mgbtnResetuj_Click(object sender, EventArgs e)
        {
            mgtxtEps.Text = ""; mgtxtX.Text = ""; mgtxtXd.Text = ""; mgtxtXg.Text = ""; mgtxtH.Text = ""; mgtxtDokładnośćObliczeń.Text = ""; mgtxtDolnaGranicaCałkowania.Text = "";
            mgtxtGórnaGranicaCałkowania.Text = "";
            mgtxtWartośćCałki.Text = ""; mgtxtWartośćFx.Text = "";
            mgtxtX.ReadOnly = false; mgtxtXd.ReadOnly = false; mgtxtXg.ReadOnly = false;
            mgtxtH.ReadOnly = false; mgtxtEps.ReadOnly = false; mgtxtWartośćCałki.ReadOnly = false;
            mgtxtWartośćFx.ReadOnly = false;
            mgtxtGrubośćLinii.Text = 1.ToString();
            mgtxtWybranyKolorLinii.BackColor = Color.Black;
            mgtxtWybranyKolorTła.BackColor = Color.Bisque;
            mgdvgTabelaFx.Visible = false; mgchWykres.Visible = false;
            mgGrafikaPowitalna.Visible = true;
            mgbtnGraficznaWizualizacjaFunkcji.Enabled = true;
            mgbtnObliczWartośćFunkcji.Enabled = true;
            mgbtnTabelarycznaWizualizacjaFunkcji.Enabled = true;
            mgbtnWartośćCałki.Enabled = true;
            stylLiniiToolStripMenuItem.Enabled = true;
            grubośćLiniiToolStripMenuItem.Enabled = true;
            
        }

        private void mgAnalizator_funkcji_Fx_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult mgPytanieDoUżytkownika = MessageBox.Show("Czy jesteś pewny/a, że chcesz zakończyć działanie programu?",
                this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);
            if (mgPytanieDoUżytkownika == DialogResult.Yes)
            {
                /* zdarzenie FormClosing powinno być zrealizowane, (czyli okno musi być zamknięte)*/
                e.Cancel = false;
            }
            else
                if (mgPytanieDoUżytkownika == DialogResult.No)
                /* zdarzenie FormClosing zostało wygenerowane przypadkowo, czyli okno (formularz) powinno zostać otwarte*/
                e.Cancel = true;
            else
                //anulowanie zdarzenia FormClosing
                e.Cancel = true;
        }

        private void mgzapiszTabelęZmianWartościFunkcjiWPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*deklaracja i utworzenie egzemplarza okna dialogowego dla wyboru (lub utworzenia) pliku do zapisu*/
            SaveFileDialog mgOknoZapisuPliku = new SaveFileDialog();
            // ustawienie filtru dla plików wyświetlanych w oknie dialogowym
            mgOknoZapisuPliku.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //wybór filtru domyślnego
            mgOknoZapisuPliku.FilterIndex = 1; //czyli filtr: *.txt
            //przywracanie bieżącego katalogu po zamknięciu okna dialogowego
            mgOknoZapisuPliku.RestoreDirectory = true;
            //domyślny wybór dysku (i ewentualnie folderu) w oknie dialogowym wyboru pliku
            mgOknoZapisuPliku.InitialDirectory = "F:\\";
            //ustalanie tytułu okna dialogowego wyboru pliku
            mgOknoZapisuPliku.Title = "Zapisanie tabeli zmian wartości funkcji w pliku";
            if (mgOknoZapisuPliku.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamWriter mgPlikZnakowy = new System.IO.StreamWriter(mgOknoZapisuPliku.FileName);
                    for (int mgi = 0; mgi < mgdvgTabelaFx.Rows.Count; mgi++)
                    {
                        mgPlikZnakowy.Write(mgdvgTabelaFx.Rows[mgi].Cells[0].Value);
                        mgPlikZnakowy.Write(';');
                        mgPlikZnakowy.WriteLine(mgdvgTabelaFx.Rows[mgi].Cells[1].Value);
                    }
                    mgPlikZnakowy.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("BŁĄD: nie można wykonać operacji na pliku " + "- (wyświetlony komunikat): " + ex.Message);
                }
            }
        }

        private void mgodczytajTabelęZmianWartościFunkcjiZPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog mgOknoOdczytuPliku = new OpenFileDialog();
            mgOknoOdczytuPliku.Filter = "txt files (*.txt) | *.txt|All files (*.*)|*.*";
            mgOknoOdczytuPliku.FilterIndex = 1;
            mgOknoOdczytuPliku.RestoreDirectory = true;
            mgOknoOdczytuPliku.InitialDirectory = "D:\\";
            mgOknoOdczytuPliku.Title = "Odczytanie (pobranie) danych z pliku";
            if (mgOknoOdczytuPliku.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamReader mgPlikZnakowy = new System.IO.StreamReader(mgOknoOdczytuPliku.FileName);
                    //wyzerowanie wierszy danych
                    mgdvgTabelaFx.Rows.Clear();
                    //wycentrowanie zapisu w kolumnach (w pierwszej i w drugiej)
                    mgdvgTabelaFx.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    mgdvgTabelaFx.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    string mgWierszdanych; //dla przechowania wczytanego z pliku wiersza danych
                    while ((mgWierszdanych = mgPlikZnakowy.ReadLine()) != null)
                    {
                        string[] mgX_Fx = mgWierszdanych.Split(';');
                        mgX_Fx[0].Trim(); mgX_Fx[1].Trim();
                        mgdvgTabelaFx.Rows.Add(mgX_Fx[0], mgX_Fx[1]);
                    }
                    //odsłonięcie kontrolki DataGridView
                    mgdvgTabelaFx.Visible = true;
                    //ukrycie grafiki powitalnej
                    mgGrafikaPowitalna.Visible = false;
                    // ukrycie kontrolki Chart
                    mgchWykres.Visible = false;
                    mgPlikZnakowy.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("BŁĄD: nie można wykonać operacji na pliku - wyświetlony komunikat:" + ex.Message);
                }
            }
        }

        private void mgAnalizator_funkcji_Fx_Load(object sender, EventArgs e)
        {
            mgzapiszTabelęZmianWartościFunkcjiWPlikuToolStripMenuItem.Enabled = false;
            koloryWykresuToolStripMenuItem.Enabled = false;
            mgtypWykresuToolStripMenuItem.Enabled = false;
        }

        private void mgexitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult mgPytanieDoUżykownika = MessageBox.Show(
                "Czy na pewno chcesz zamknąć formularz (co może skutkować utratą danych zapisanych na formularzu)?",
                this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);
            if (mgPytanieDoUżykownika == DialogResult.Yes)
                Close();
        }

        private void mgtoolStripMenuItem2_Click(object sender, EventArgs e)
        {

            mgchWykres.Series[0].BorderWidth = 1;
        }

        private void mgtoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderWidth = 2;
        }

        private void mgtoolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderWidth = 3;
        }

        private void mgtoolStripMenuItem5_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderWidth = 4;
        }

        private void mgtoolStripMenuItem6_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderWidth = 5;
        }

        private void mgliniowyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void mgkropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderDashStyle=ChartDashStyle.Dot;
        }

        private void mgkreskowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderDashStyle = ChartDashStyle.Dash;
        }

        private void mgkreskowokropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
        }

        private void mgciągłaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mgchWykres.Series[0].BorderDashStyle = ChartDashStyle.Solid;
        }

        private void mgkolorliniiWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog mgOknoKolorów = new ColorDialog();
            mgOknoKolorów.Color = mgchWykres.Series[0].Color;
            if (mgOknoKolorów.ShowDialog() == DialogResult.OK)
            {
                //ustawienie nowego koloru w wzierniku koloru linii
                mgtxtWybranyKolorLinii.BackColor = mgOknoKolorów.Color;
                //ustawienie nowego koloru linii wykresu
                mgchWykres.Series[0].Color = mgOknoKolorów.Color;
            }
        }

        private void mgkolorczcionkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog mgOknoKolorów = new ColorDialog();
            mgOknoKolorów.Color = mgchWykres.Series[0].Color;
            if (mgOknoKolorów.ShowDialog() == DialogResult.OK)
            {
                //ustawienie nowego koloru czcionki wykresu
                  mgchWykres.ChartAreas[0].AxisX.LabelStyle.ForeColor = mgOknoKolorów.Color;
                  mgchWykres.ChartAreas[0].AxisY.LabelStyle.ForeColor = mgOknoKolorów.Color;
            }

        }

        private void mgbtnWartośćCałki_Click(object sender, EventArgs e)
        {
            float mgxp = 0.0F; float mgxk = 0.0F; float mgn = 0.0F;
            if (String.IsNullOrEmpty(mgtxtGórnaGranicaCałkowania.Text))
            {
                //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtGórnaGranicaCałkowania, "BŁĄD: musisz podać wartość górnej granicy całkowania");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK

            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            if (!float.TryParse(mgtxtGórnaGranicaCałkowania.Text, out mgxp))
            { //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtGórnaGranicaCałkowania, "BŁĄD: wystąpił niedozwolony znak w zapisie górnej granicy całkowania");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }
            else
                mgerrorProvider1.Dispose();
            if (String.IsNullOrEmpty(mgtxtDolnaGranicaCałkowania.Text))
            {
                //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtDolnaGranicaCałkowania, "BŁĄD: musisz podać wartość dolnej granicy całkowania");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK

            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            if (!float.TryParse(mgtxtDolnaGranicaCałkowania.Text, out mgxk))
            { //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtDolnaGranicaCałkowania, "BŁĄD: wystąpił niedozwolony znak w zapisie dolnej granicy całkowania");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }
            else
                mgerrorProvider1.Dispose();

            if (String.IsNullOrEmpty(mgtxtDokładnośćObliczeń.Text))
            {
                //Zapalenie kontrolki errorProvider (sygnalizacja błędu)
                mgerrorProvider1.SetError(mgtxtDokładnośćObliczeń, "BŁĄD: musisz podać dokładność obliczeń!");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK

            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            if (!float.TryParse(mgtxtDokładnośćObliczeń.Text, out mgn))
            {
                mgerrorProvider1.SetError(mgtxtDokładnośćObliczeń, "BŁĄD: wystąpił niedozwolony znak w zapisie wartości dokładności");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }
            else
                mgerrorProvider1.Dispose(); //zgaszenie kontrolki errorProvider1
            if ((mgn <= 0.0f) || (mgn >= 1.0f))
            {
                mgerrorProvider1.SetError(mgtxtDokładnośćObliczeń, "BŁĄD: dokładność obliczeń musi spełniać warunek wejściowy: 0.0 < Dokładność <1.0");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }
            if (mgxk>mgxp)
            {
                mgerrorProvider1.SetError(mgtxtDolnaGranicaCałkowania, "BŁĄD: Dolna granica całkowania musi być mniejsza od górnej granicy całkowania");
                return; //Zakończenie pobierania danych wejściowych i przerwanie obsługi zdarzenia CLICK
            }

            //float mgObliczonaWartośćCałki;
            ///*  obliczenie wartości funkcji F(X), czyli wyznaczenie sumy podanego szeregu funkcyjnego (z dokładnością Eps) */
            //mgObliczonaWartośćCałki = (float)mgcalculate(mgxp, mgxk, mgn, mgfunction);

            ////wizualizacja obliczonej wartości funkcji F(X)
            //// mgtxtWartośćCałki.Text = Math.Round(mgObliczonaWartośćCałki, 3, MidpointRounding.AwayFromZero).ToString();
            //mgtxtWartośćCałki.Text = mgcalculate(mgxp, mgxk, mgn, mgfunction).ToString();

            ///*ustawienie stanu braku aktywności przycisku poleceń: Obliczenie wartości całki*/
            mgbtnWartośćCałki.Enabled = false;
            //Nie mogę sobie poradzić z całkowaniem, kod jest zakomentowany
        }

        private void mgtxtEps_TextChanged(object sender, EventArgs e)
        {

        }

        private void mgtxtWartośćFx_TextChanged(object sender, EventArgs e)
        {

        }

        private void mgtxtWybranyKolorLinii_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
