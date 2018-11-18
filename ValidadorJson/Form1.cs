using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
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

            textBox2.Text = "";

            var jsonRecebido = textBox1.Text;

            try
            {
                var resultadoDoParse = JSchema.Parse(jsonRecebido);
            }
            catch (JsonReaderException ex)
            {
                var lineNumber = ex.LineNumber;
                var linePosition = ex.LinePosition;

                textBox2.Text = string.Format("JSON inválido na linha {0} e na coluna {1}.", lineNumber, linePosition);

                return;
            }
           
            var parseObj = JObject.Parse(jsonRecebido);

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
       

        private void Form1_Load(object sender, EventArgs e)
        {
            notasFiscais = new NotaFiscal[2];

            JSchema schemaNFSe = JSchema.Parse(@"{
                                            'type': 'object',
                                            'properties': {
                                                'idExterno': { 'type':'string'},
                                                'ambienteEmissao': { 'type':'string'},
                                                'enviarPorEmail': {'type': [ 'boolean', 'null' ]},
                                                'cliente': {
                                                    'type': 'object',
                                                    'properties': {
                                                        'nome': { 'type':'string'},
                                                        'tipoPessoa': {'type':'string'},
                                                        'email': {'type':[ 'string', 'null' ]},
                                                        'cpfCnpj': {'type':'string'},
                                                        'inscricaoMunicipal': {'type':'string'},
                                                        'inscricaoEstadual': {'type':'string'},
                                                        'telefone': {'type':'string'},
                                                        'endereco': {
                                                            'type': 'object',
                                                            'pais': {'type':'string'},
                                                            'logradouro': {'type':'string'},
                                                            'uf': {'type':'string'},
                                                            'cidade': {'type':'string'},
                                                            'numero': {'type':'string'},
                                                            'complemento': {'type':'string'},
                                                            'bairro': {'type':'string'},
                                                            'cep': {'type':'string'}
                                                           },
                                                        },
                                                    'required': ['nome'],
                                                    'servico': {
                                                        'type': 'object',
                                                        'properties': {
                                                            'descricao': {'type':'string'},
                                                            'aliquotaIss': {'type':'float'},
                                                            'codigoInternoServicoMunicipal': {'type':'integer'},
                                                            'issRetidoFonte': {'type':'boolean'},
                                                            'valorCofins': {'type':'float'},
                                                            'valorCsll': {'type':'float'},
                                                            'valorInss': {'type':'float'},
                                                            'valorIr': {'type':'float'},
                                                            'valorPis': {'type':'float'}
                                                          }
                                                    },
                                                   'valorTotal': {'type':'float'},
                                                 }
                                             },
                                            'required': ['idExterno', 'ambienteEmissao']
                                        }");
            NotaFiscal notaFiscalSe = new NotaFiscal("NFSe", schemaNFSe);
            JSchema schemaNFC = JSchema.Parse(@"{'type': 'object',
                      'properties': {
                        'id': {
                          'type': 'string'
                        },
                        'ambienteEmissao': {
                          'type': 'string'
                        },
                        'pedido': {
                          'type': 'object',
                          'properties': {
                            'presencaConsumidor': {
                              'type': 'string'
                            },
                            'pagamento': {
                              'type': 'object',
                              'properties': {
                                'tipo': {
                                  'type': 'string'
                                },
                                'formas': {
                                  'type': 'array',
                                  'items': [
                                    {
                                      'type': 'object',
                                      'properties': {
                                        'tipo': {
                                          'type': 'string'
                                        },
                                        'valor': {
                                          'type': 'number'
                                        }
                                      },
                                      'required': [
                                        'tipo',
                                        'valor'
                                      ]
                                    },
                                    {
                                      'type': 'object',
                                      'properties': {
                                        'tipo': {
                                          'type': 'string'
                                        },
                                        'valor': {
                                          'type': 'number'
                                        },
                                        'credenciadoraCartao': {
                                          'type': 'object',
                                          'properties': {
                                            'tipoIntegracaoPagamento': {
                                              'type': 'string'
                                            },
                                            'cnpjCredenciadoraCartao': {
                                              'type': 'string'
                                            },
                                            'bandeira': {
                                              'type': 'string'
                                            },
                                            'autorizacao': {
                                              'type': 'string'
                                            }
                                          },
                                          'required': [
                                            'tipoIntegracaoPagamento',
                                            'cnpjCredenciadoraCartao',
                                            'bandeira',
                                            'autorizacao'
                                          ]
                                        }
                                      },
                                      'required': [
                                        'tipo',
                                        'valor',
                                        'credenciadoraCartao'
                                      ]
                                    }
                                  ]
                                }
                              },
                              'required': [
                                'tipo',
                                'formas'
                              ]
                            }
                          },
                          'required': [
                            'presencaConsumidor',
                            'pagamento'
                          ]
                        },
                        'cliente': {
                          'type': 'object',
                          'properties': {
                            'tipoPessoa': {
                              'type': 'string'
                            },
                            'nome': {
                              'type': 'string'
                            },
                            'email': {
                              'type': 'string'
                            },
                            'telefone': {
                              'type': 'string'
                            },
                            'cpfCnpj': {
                              'type': 'string'
                            },
                            'endereco': {
                              'type': 'object',
                              'properties': {
                                'pais': {
                                  'type': 'string'
                                },
                                'uf': {
                                  'type': 'string'
                                },
                                'cidade': {
                                  'type': 'string'
                                },
                                'logradouro': {
                                  'type': 'string'
                                },
                                'numero': {
                                  'type': 'string'
                                },
                                'complemento': {
                                  'type': 'string'
                                },
                                'bairro': {
                                  'type': 'string'
                                },
                                'cep': {
                                  'type': 'string'
                                }
                              },
                              'required': [
                                'pais',
                                'uf',
                                'cidade',
                                'logradouro',
                                'numero',
                                'complemento',
                                'bairro',
                                'cep'
                              ]
                            }
                          },
                          'required': [
                            'tipoPessoa',
                            'nome',
                            'email',
                            'telefone',
                            'cpfCnpj',
                            'endereco'
                          ]
                        },
                        'enviarPorEmail': {
                          'type': 'boolean'
                        },
                        'itens': {
                          'type': 'array',
                          'items': [
                            {
                              'type': 'object',
                              'properties': {
                                'cfop': {
                                  'type': 'string'
                                },
                                'codigo': {
                                  'type': 'string'
                                },
                                'descricao': {
                                  'type': 'string'
                                },
                                'ncm': {
                                  'type': 'string'
                                },
                                'quantidade': {
                                  'type': 'integer'
                                },
                                'unidadeMedida': {
                                  'type': 'string'
                                },
                                'valorUnitario': {
                                  'type': 'number'
                                },
                                'descontos': {
                                  'type': 'number'
                                },
                                'impostos': {
                                  'type': 'object',
                                  'properties': {
                                    'percentualAproximadoTributos': {
                                      'type': 'object',
                                      'properties': {
                                        'detalhado': {
                                          'type': 'object',
                                          'properties': {
                                            'percentualFederal': {
                                              'type': 'number'
                                            },
                                            'percentualEstadual': {
                                              'type': 'number'
                                            },
                                            'percentualMunicipal': {
                                              'type': 'number'
                                            }
                                          },
                                          'required': [
                                            'percentualFederal',
                                            'percentualEstadual',
                                            'percentualMunicipal'
                                          ]
                                        },
                                        'fonte': {
                                          'type': 'string'
                                        }
                                      },
                                      'required': [
                                        'detalhado',
                                        'fonte'
                                      ]
                                    },
                                    'icms': {
                                      'type': 'object',
                                      'properties': {
                                        'situacaoTributaria': {
                                          'type': 'string'
                                        },
                                        'origem': {
                                          'type': 'integer'
                                        },
                                        'aliquota': {
                                          'type': 'number'
                                        }
                                      },
                                      'required': [
                                        'situacaoTributaria',
                                        'origem',
                                        'aliquota'
                                      ]
                                    }
                                  },
                                  'required': [
                                    'percentualAproximadoTributos',
                                    'icms'
                                  ]
                                }
                              },
                              'required': [
                                'cfop',
                                'codigo',
                                'descricao',
                                'ncm',
                                'quantidade',
                                'unidadeMedida',
                                'valorUnitario',
                                'descontos',
                                'impostos'
                              ]
                            }
                          ]
                        }
                      },
                      'required': [
                        'id',
                        'ambienteEmissao',
                        'pedido',
                        'cliente',
                        'enviarPorEmail',
                        'itens'
                      ]
                }");
            NotaFiscal notaFiscalC = new NotaFiscal("NFC", schemaNFC);
            notasFiscais[0] = notaFiscalSe;
            notasFiscais[1] = notaFiscalC;
            tipoNota.Items.Add(notaFiscalSe.tipo);
            tipoNota.Items.Add(notaFiscalC.tipo);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tipoNota_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = tipoNota.SelectedIndex;
            notaFiscalSelecionada = this.notasFiscais[indice];
        }
    }
}
