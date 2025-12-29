// See https://aka.ms/new-console-template for more information
using Reinventa.Utilitarios;

//Int64[] documentos = new Int64[]
//{
//    20455067465
//};

//foreach (var documentosFaltantes in documentos)
//{
//    string original = documentosFaltantes.ToString();
//    string encriptado = CryptoHelper.Encrypt(original);
//    Console.WriteLine(string.Concat(documentosFaltantes.ToString(), "-",encriptado));
//}


var empresas = new (long Ruc, string RazonSocial)[]
{
     (20100754755, "IVC CONTRATISTAS GENERALES SA"),
    (20535689394, "OVERPRIME MANUFACTURING S.A.C"),
    (20523319605, "UMI FOODS SAC"),
    (20456161007, "B & H CARDENAS S.A.C."),
    (20563014700, "AEMSYS SAC"),
    (20530240178, "POLIMEROS DEL NORTE S.A.C"),
    (20514429384, "PRAXIS ECOLOGY SAC"),
    (20537471248, "UNION DE TECNICOS ELECTROMECANICOS S.A.C."),
    (20426138868, "SUMINISTROS AVICOLAS S A C"),
    (20604296481, "VALLE SAN MIGUEL SAC"),
    (20545889685, "CORRALES & CIA SAC")
};


foreach (var e in empresas)
{
 
    string original = e.Ruc.ToString();
    string encriptado = CryptoHelper.Encrypt(original);
    Console.WriteLine(string.Concat("INSERT INTO HUELLACARBONO_CLIENTE VALUES(", e.Ruc, ",'", e.RazonSocial.ToString(), "','", encriptado, "',1,0);"));

}


//string desencriptado = CryptoHelper.Decrypt(encriptado);
//Console.WriteLine(desencriptado);