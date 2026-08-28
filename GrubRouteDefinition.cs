using System.Collections.Generic;

namespace HallownestWayfinder
{
    /// <summary>
    /// Complete vanilla grub checklist. Scene identifiers are based on the
    /// canonical ItemChanger location data and the game's scenesGrubRescued list.
    /// </summary>
    public static class GrubRouteDefinition
    {
        public const string Name = "Larvas 46/46";

        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            G("grub_crossroads_acid", "Encruzilhada • Passagem com ácido", "A partir de Cornifer, siga pela passagem oeste até a jarra acima do ácido.", "Crossroads_35", 270f),
            G("grub_crossroads_center", "Encruzilhada • Sala central", "Use o dash para alcançar a larva na sala central da Encruzilhada.", "Crossroads_05", 90f),
            G("grub_crossroads_stag", "Encruzilhada • Próxima à estação", "Saia da estação pela direita, suba e atravesse a parede quebrável à esquerda.", "Crossroads_03", 0f),
            G("grub_crossroads_spike", "Encruzilhada • Corredor de espinhos", "Procure a larva no corredor de espinhos da parte sudeste da região.", "Crossroads_31", 135f),
            G("grub_crossroads_guarded", "Encruzilhada • Larva protegida", "Suba pela área oriental e derrote o inimigo que guarda a jarra.", "Crossroads_48", 45f),

            G("grub_greenpath_cornifer", "Caminho Verde • Próxima a Cornifer", "Depois de ver Hornet, corte o cipó da passagem secreta e siga até a larva.", "Fungus1_06", 135f),
            G("grub_greenpath_journal", "Caminho Verde • Próxima ao Caçador", "Desça a partir da cabana do Caçador e explore a parte inferior.", "Fungus1_07", 225f),
            G("grub_greenpath_mmc", "Caminho Verde • Caminho de Sheo", "Explore os corredores inferiores no caminho para a casa de Sheo.", "Fungus1_13", 270f),
            G("grub_greenpath_stag", "Caminho Verde • Cavaleiro do Musgo", "Derrote o Cavaleiro do Musgo que protege a jarra na parte superior.", "Fungus1_21", 90f),
            G("grub_cliffs", "Penhascos Uivantes", "Vasculhe a passagem escondida dos Penhascos Uivantes.", "Fungus1_28", 315f),

            G("grub_fungal_bouncy", "Ermos Fúngicos • Cogumelos elásticos", "Use a Garra e os cogumelos elásticos nos túneis próximos à Estação da Rainha.", "Fungus2_18", 0f),
            G("grub_fungal_spore", "Ermos Fúngicos • Cogumelo com Esporos", "Atravesse o salão de ácido próximo ao amuleto Cogumelo com Esporos.", "Fungus2_20", 270f),
            G("grub_deepnest_spike", "Ninho Profundo • Piso frágil", "Caia pelo piso frágil dos Ermos e encontre a larva verdadeira entre os imitadores.", "Deepnest_03", 180f),

            G("grub_city_left", "Cidade das Lágrimas • Torre oeste", "Suba pelos elevadores acima de Lemm e entre na sala lateral.", "Ruins1_05", 45f),
            G("grub_soul_sanctum", "Cidade das Lágrimas • Santuário das Almas", "Após o Mestre das Almas, procure a jarra antes de quebrar o último piso.", "Ruins1_32", 180f),
            G("grub_city_guarded", "Cidade das Lágrimas • Larva protegida", "Explore o edifício isolado da cidade e derrote os guardas.", "Ruins_House_01", 90f),

            G("grub_peak_spike", "Pico de Cristal • Esteiras e espinhos", "Suba pelas esteiras verticais, atravesse os espinhos e use a alavanca.", "Mines_03", 0f),
            G("grub_peak_chest", "Pico de Cristal • Abaixo do baú", "Procure a passagem inferior escondida sob a área do baú.", "Mines_04", 180f),
            G("grub_peak_mimic", "Pico de Cristal • Mimic", "Quebre as paredes falsas e confirme a larva verdadeira entre os imitadores.", "Mines_16", 90f),
            G("grub_peak_crushers", "Pico de Cristal • Trituradores", "Atravesse o desafio dos pistões e trituradores móveis.", "Mines_19", 90f),
            G("grub_peak_crown", "Pico de Cristal • Coroa de Hallownest", "Suba em direção ao topo do Pico e explore o caminho da Coroa.", "Mines_24", 315f),
            G("grub_peak_heart", "Pico de Cristal • Coração de Cristal", "Use o super dash para atravessar o abismo próximo à habilidade.", "Mines_31", 270f),
            G("grub_mound", "Pico de Cristal • Monte Cristalizado", "Explore o Monte Cristalizado próximo ao Xamã e à Escuridão Descente.", "Mines_35", 90f),

            G("grub_resting", "Campos de Descanso", "Quebre o sarcófago e atravesse os túneis ocultos abaixo dos Campos.", "RestingGrounds_10", 180f),
            G("grub_waterways_main", "Esgotos Reais • Corredor principal", "A partir do banco inferior, suba e atravesse a parede quebrável.", "Waterways_04", 270f),
            G("grub_isma", "Esgotos Reais • Bosque de Isma", "Após obter a Lágrima de Isma, nade pelo ácido e suba pela parede.", "Waterways_13", 0f),
            G("grub_waterways_tram", "Esgotos Reais • Próxima ao bonde", "Use o acesso do bonde para alcançar a área isolada dos Esgotos.", "Waterways_14", 90f),

            G("grub_basin_dive", "Bacia Antiga • Mergulho Desolador", "Use o mergulho no piso solto próximo aos Mawleks menores.", "Abyss_17", 180f),
            G("grub_basin_wings", "Bacia Antiga • Asas do Monarca", "Use o pulo duplo para alcançar a jarra na parte oeste da Bacia.", "Abyss_19", 270f),

            G("grub_dark_deepnest", "Ninho Profundo • Sala escura", "Leve a Lanterna e vasculhe os corredores escuros do Ninho.", "Deepnest_39", 270f),
            G("grub_deepnest_mimic", "Ninho Profundo • Larvas imitadoras", "Encontre a larva verdadeira entre as criaturas imitadoras.", "Deepnest_36", 270f),
            G("grub_deepnest_nosk", "Ninho Profundo • Caminho de Nosk", "Explore as passagens quebráveis próximas às Termas e ao caminho de Nosk.", "Deepnest_31", 270f),
            G("grub_beasts_den", "Ninho Profundo • Toca da Besta", "Explore a Toca da Besta antes de libertar Herrah.", "Deepnest_Spider_Town", 0f),

            G("grub_kingdom_camp", "Borda do Reino • Acampamento", "Explore a região próxima ao acampamento e às grandes carcaças.", "Deepnest_East_11", 90f),
            G("grub_kingdom_oro", "Borda do Reino • Próxima a Oro", "Atravesse os corredores de Grandes Saltadores próximos à casa de Oro.", "Deepnest_East_14", 90f),
            G("grub_kings_station", "Cidade das Lágrimas • Estação do Rei", "Procure a sala escondida nos corredores próximos à Estação do Rei.", "Ruins2_07", 90f),

            G("grub_gardens_stag", "Jardins da Rainha • Estação", "Explore os corredores próximos à estação dos Jardins.", "Fungus3_10", 90f),
            G("grub_gardens_top", "Jardins da Rainha • Parte superior", "Use as habilidades de movimento para atravessar os espinhos superiores.", "Fungus3_22", 0f),
            G("grub_gardens_marmu", "Jardins da Rainha • Próxima a Marmu", "Procure a jarra nos corredores ao redor do túmulo de Marmu.", "Fungus3_48", 90f),
            G("grub_fog", "Cânion da Névoa", "Atravesse as bolhas explosivas e procure a passagem isolada do Cânion.", "Fungus3_47", 270f),

            G("grub_hive_external", "Colmeia • Entrada", "Quebre a parede do túnel do bonde e explore a parte externa da Colmeia.", "Hive_03", 90f),
            G("grub_hive_internal", "Colmeia • Interior", "Atravesse as grandes câmaras internas usando os insetos como plataformas.", "Hive_04", 90f),

            G("grub_collector", "Torre do Amor • Três larvas", "Derrote o Colecionador e quebre as três jarras no salão superior.", "Ruins2_11", 0f, count: 3),
            G("grub_watcher", "Torre do Vigia", "Abra as alavancas e procure a sala acima da saída direita da torre.", "Ruins2_03", 0f)
        };

        private static RouteStep G(string id, string title, string hint, string scene,
            float arrow, int count = 1)
        {
            return new RouteStep
            {
                Id = id,
                Title = title,
                Hint = hint,
                Icon = "grub.png",
                RequiredGrubScene = scene,
                RequiredGrubCountInScene = count,
                ArrowDegrees = arrow
            };
        }
    }
}
