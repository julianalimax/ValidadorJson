using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eNotasJsonValidator.Core.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace eNotasJsonValidator.Core
{
    public class NFSeJsonValidator : BaseJsonValidator
    {
        public override ValidationResult Validate(string json)
        {
            try
            {
                var resultadoDoParse = JSchema.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                var lineNumber = ex.LineNumber;
                var linePosition = ex.LinePosition;

                textBox2.Text = string.Format("JSON inválido na linha {0} e na coluna {1}.", lineNumber, linePosition);

                return;
            }

            var parseObj = JObject.Parse(json);

            IList<ValidationError> errors;

            var valid = parseObj.IsValid(notaFiscalSelecionada.schema, out errors);

            if (valid)
            {
                string formatted = JsonFormatting.Ident(jsonRecebido);
                textBox2.Text = "JSON Válido";
                textBox1.Text = formatted.ToString();
            }
            else
            {
                var strBuilder = new StringBuilder();

                foreach (var error in errors)
                {
                    if (error.ErrorType == ErrorType.Type)
                    {
                        strBuilder.AppendLine(string.Format("Erro no campo {0} esperando {1} e veio {2} na linha {3} na coluna {4}",
                            error.Path,
                            error.Schema.Type.Value.ToString(),
                            error.Value != null ? error.Value.GetType().ToString() : "null",
                            error.LineNumber,
                            error.LinePosition));
                    }
                    else if (error.ErrorType == ErrorType.Required)
                    {
                        var campos = string.Join(",", ((IList<string>)error.Value));

                        if (string.IsNullOrEmpty(error.Path))
                        {
                            strBuilder.AppendLine(string.Format("Campo {0} obrigatório na raiz", campos));
                        }
                        else
                        {
                            strBuilder.AppendLine(string.Format("Campo {0} obrigatório dentro de {1}", campos, error.Path));
                        }
                    }
                }

                textBox2.Text = strBuilder.ToString();
            }
        }
    }
}
