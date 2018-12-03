using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using eNotasJsonValidator.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace ValidadorJson
{
    public partial class Form1 : Form
    {
        public NotaFiscal notaFiscalSelecionada;
        private NotaFiscal[] notasFiscais;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            var receivedJson = textBox1.Text;

            var nfseValidator = new NFSeJsonValidator();

            var result = nfseValidator.Validate(receivedJson);

            if (result.IsValid)
            {
                textBox2.Text = "JSON Válido";
                textBox1.Text = JsonFormatting.Ident(receivedJson);
            }
            else
            {
                var validationErrors = result.Errors;
                textBox2.Text = validationErrors.ToString();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //notasFiscais = new NotaFiscal[3];

            //JSchema schemaNFSe = JSchema.Parse(@"");
            //NotaFiscal notaFiscalSe = new NotaFiscal("NFSe", schemaNFSe);
            //JSchema schemaNFC = JSchema.Parse(@"");

            //NotaFiscal notaFiscalP = new NotaFiscal("NF", schemaNF);
            //notasFiscais[0] = notaFiscalSe;
            //notasFiscais[1] = notaFiscalC;
            //notasFiscais[2] = notaFiscalP;

            //tipoNota.Items.Add(notaFiscalSe.tipo);
            //tipoNota.Items.Add(notaFiscalC.tipo);
            //tipoNota.Items.Add(notaFiscalP.tipo);


        }

        private void tipoNota_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = tipoNota.SelectedIndex;
            notaFiscalSelecionada = this.notasFiscais[indice];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }
    }
}
