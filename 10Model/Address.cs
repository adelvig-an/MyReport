namespace _10Model
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressFull { get; set; } //Адрес полностью
        public string Index { get; set; } //Индекс
        public string Country { get; set; } //Страна
        public string FederalDistrict { get; set; } //Федеральный округ
        public string RegionKladrId { get; set; } //КЛАДР-код региона
        public string RegionWthType { get; set; } //Регион с типом
        public string RegionType { get; set; } //Тип региона (сокращенный)
        public string RegionTypeFull { get; set; } //Тип региона
        public string Region { get; set; } //Регион
        public string AreaKladrId { get; set; } //КЛАДР-код района
        public string AreaWithType { get; set; } //Район в регионе с типом
        public string AreaType { get; set; } //Тип района в регионе (сокращенный)
        public string AreaTypeFull { get; set; } //Тип района в регионе
        public string Area { get; set; } //Район в регионе
        public string CityKladrId { get; set; } //КЛАДР-код города
        public string CityWithType { get; set; } //Город с типом
        public string CityType { get; set; } //Тип города (сокращенный)
        public string CityTypeFull { get; set; } //Тип города
        public string City { get; set; } //Город
        public string CityDistrictWithType { get; set; } //Район города с типом
        public string CityDistrictType { get; set; } //Тип района города (сокращенный)
        public string CityDistrictTypeFull { get; set; } //Тип района города
        public string CityDistrict { get; set; } //Район города
        public string SettlementKladrId { get; set; } //КЛАДР-код населенного пункта
        public string SettlemenWithType { get; set; } //Населенный пункт с типом
        public string SettlemenType { get; set; } //Тип населенного пункта (сокращенный)
        public string SettlemenTypeFull { get; set; } //Тип населенного пункта
        public string Settlemen { get; set; } //Населенный пункт
        public string StreetKladrId { get; set; } //КЛАДР-код улицы
        public string StreetWithType { get; set; } //Улица с типом
        public string StreetType { get; set; } //Тип улицы (сокращенный)
        public string StreetTypeFull { get; set; } //Тип улицы
        public string Street { get; set; } //Улица
        public string HouseKladrId { get; set; } //КЛАДР-код дома
        public string HouseType { get; set; } //Тип дома (сокращенный)
        public string HouseTypeFull { get; set; } //Тип дома
        public string House { get; set; } //Номер дома
        public string BlockType { get; set; } //Тип корпуса/строения (сокращенный)
        public string BloctTypeFull { get; set; } //Тип корпуса/строения
        public string Block { get; set; } //Номер корпуса/строения
        public string Entrance { get; set; } //Подъезд
        public string Floor { get; set; } //Этаж
        public string FlatType { get; set; } //Тип квартиры (сокращенный)
        public string FlatTypeFull { get; set; } //Тип квартиры
        public string Flat { get; set; } //Квартиры
    }
}
