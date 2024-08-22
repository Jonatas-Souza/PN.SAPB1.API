namespace PN.SAPB1.API.Models
{
    public class RetornoCnpj
    {
        public string? status { get; set; }
        public string? ultima_atualizacao { get; set; }
        public string? cnpj { get; set; }
        public string? tipo { get; set; }
        public string? porte { get; set; }
        public string? nome { get; set; }
        public string? fantasia { get; set; }
        public string? abertura { get; set; }
        public List<Atividade_Principal> atividade_principal { get; set; } = new List<Atividade_Principal>();
        public List<Atividades_Secundarias> atividades_secundarias { get; set; } = new List<Atividades_Secundarias>();
        public string? natureza_juridica { get; set; }
        public string? logradouro { get; set; }
        public string? numero { get; set; }
        public string? complemento { get; set; }
        public string? cep { get; set; }
        public string? bairro { get; set; }
        public string? municipio { get; set; }
        public string? uf { get; set; }
        public string? email { get; set; }
        public string? telefone { get; set; }
        public string? efr { get; set; }
        public string? situacao { get; set; }
        public string? data_situacao { get; set; }
        public string? motivo_situacao { get; set; }
        public string? situacao_especial { get; set; }
        public string? data_situacao_especial { get; set; }
        public string? capital_social { get; set; }
        public List<Qsa> qsa { get; set; } = new List<Qsa>();
        public Billing billing { get; set; }
    }

    public class Billing
    {
        public bool free { get; set; }
        public bool database { get; set; }
    }

    public class Atividade_Principal
    {
        public string? code { get; set; }
        public string? text { get; set; }
    }

    public class Atividades_Secundarias
    {
        public string? code { get; set; }
        public string? text { get; set; }
    }

    public class Qsa
    {
        public string? nome { get; set; }
        public string? qual { get; set; }
        public string? pais_origem { get; set; }
        public string? nome_rep_legal { get; set; }
        public string? qual_rep_legal { get; set; }
    }

}
