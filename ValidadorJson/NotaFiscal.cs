using Newtonsoft.Json.Schema;
using System;

public class NotaFiscal
{
    public String tipo;
    public JSchema schema;

    public NotaFiscal(String tipo, JSchema schema)
    {
        this.tipo = tipo;
        this.schema = schema;
    }
}
