namespace AktienVergleich.Models
{
    public class Aktie
    {
        #region Eingabe-Properties

        /// <summary>
        /// Der Name der Aktie
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Der Interval (in Anzahl der Monate) der Dividenden-Ausschüttung
        /// 
        /// 1 = Monatlich
        /// 3 = Quartalweise
        /// 6 = Halbjährlich
        /// 12 = Jährlich
        /// </summary>
        public int Interval { get; set; } = 0;
        /// <summary>
        /// Der Preis einer einzelnen Aktie
        /// </summary>
        public double Price { get; set; } = 0.00;
        /// <summary>
        /// Die Höhe der Dividende
        /// </summary>
        public double DividendSum { get; set; } = 0.00;

        #endregion

        #region Ausgabe-Properties

        /// <summary>
        /// Gibt die berechnete Dividende pro Monat an (zum Vergleich)
        /// </summary>
        public double DividendPerMonthSum { get; set; } = 0.00;

        #endregion

        #region constructors

        public Aktie()
        {

        }

        #endregion

        #region logic



        #endregion
    }
}
