using System.Collections.Generic;

namespace HallownestWayfinder
{
    /// <summary>
    /// Safe, glitchless route for the Speedrun 1 achievement, adapted from
    /// fireb0rn's Steam guide. Hints are original summaries for the in-game HUD.
    /// </summary>
    public static class SpeedrunRouteDefinition
    {
        public const string Name = "Speedrun 5h • Sem glitches";

        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            // Segmento 1 — Espírito Vingativo
            S("sr01_fury", "Seg. 01 • Pegar Fúria dos Caídos", "Antes de Dirtmouth, atravesse os espinhos da Passagem do Rei com ataques para baixo.", "Fury_of_the_Fallen.png", pb: "gotCharm_6", target: "Tutorial_01", arrow: 135f),
            S("sr01_geo", "Seg. 01 • Juntar 50 Geo", "Quebre os depósitos de Geo no caminho até a estação da Encruzilhada. Evite parar para farmar inimigos comuns.", "crawlid.png", arrow: 225f),
            S("sr01_stag", "Seg. 01 • Abrir a estação da Encruzilhada", "Pague 50 Geo, toque o sino e use o banco apenas se precisar recuperar vida.", "LastStag.png", pb: "openedCrossroads", target: "Crossroads_47", arrow: 225f),
            S("sr01_false_knight", "Seg. 01 • Derrotar o Falso Cavaleiro", "Derrote o chefe, pegue o Brasão da Cidade e abra o baú de Geo.", "False_Knight.png", pb: "killedFalseKnight", target: "Crossroads_10", arrow: 315f),
            S("sr01_spell", "Seg. 01 • Obter Espírito Vingativo", "Siga o Xamã, conclua o Monte Ancestral e receba seu primeiro feitiço.", "Vengeful_Spirit_Icon.png", pb: "hasSpell", target: "Crossroads_ShamanTemple", arrow: 270f),
            S("sr01_soul_catcher", "Seg. 01 • Pegar e equipar Apanhador de Almas", "Pegue o amuleto após o Baldur Ancião e equipe-o no banco antes de seguir ao Caminho Verde.", "Soul_Catcher.png", pb: "gotCharm_20", target: "Crossroads_ShamanTemple", arrow: 90f),

            // Segmento 2 — Manto de Asa de Mariposa
            S("sr02_vengefly", "Seg. 02 • Derrotar Rei Vengemosca", "No caminho pela parte alta do Caminho Verde, derrote o Rei Vengemosca para obter Geo rápido.", "Vengefly_King_Zote.png", pb: "zoteRescuedBuzzer", target: "Fungus1_20_v02", arrow: 315f),
            S("sr02_journal", "Seg. 02 • Pegar o Diário do Viajante", "Quebre a parede antes da arena de Hornet e recolha o diário para vender mais tarde.", "Wanderers_Journal.png", pi: "trinket1", min: 1, target: "Fungus1_22", arrow: 270f),
            S("sr02_bench", "Seg. 02 • Usar o banco do Caminho Verde", "Sente no banco da estação, mas não gaste Geo liberando o Último Besouro.", "bench.png", target: "Fungus1_16_alt", arrow: 180f),
            S("sr02_hornet", "Seg. 02 • Derrotar Hornet e obter o dash", "Derrote Hornet, pegue o Manto de Asa de Mariposa e faça save & quit assim que a coleta terminar.", "Hornet.png", pb: "hasDash", target: "Fungus1_04", arrow: 270f),

            // Segmento 3 — Garra de Louva-a-Deus
            S("sr03_queen_bench", "Seg. 03 • Usar o banco da Estação da Rainha", "Atravesse o Cânion da Névoa e sente no banco da estação antes de entrar nos Ermos Fúngicos.", "bench.png", arrow: 180f),
            S("sr03_seal", "Seg. 03 • Pegar o Selo de Hallownest", "Recolha o selo indicado pela rota; ele ajudará a pagar as compras na Cidade das Lágrimas.", "Wanderers_Journal.png", arrow: 90f),
            S("sr03_claw", "Seg. 03 • Obter Garra de Louva-a-Deus", "Atravesse os cogumelos elásticos, acione a alavanca da vila e pegue a habilidade de escalar paredes.", "Mantis_Claw.png", pb: "hasWalljump", arrow: 270f),
            S("sr03_city", "Seg. 03 • Entrar na Cidade das Lágrimas", "Siga pelo portão oriental dos Ermos, pegando o diário e os depósitos de Geo no caminho.", "crawlid.png", pb: "visitedRuins", arrow: 90f),

            // Segmento 4 — Santuário das Almas
            S("sr04_nail", "Seg. 04 • Comprar a primeira melhoria do ferrão", "Use o banco inferior, pague 250 Geo ao Ferreiro e faça save & quit para voltar rapidamente.", "Nailsmith.png", pi: "nailSmithUpgrades", min: 1, arrow: 270f),
            S("sr04_seal", "Seg. 04 • Pegar o selo a caminho do Santuário", "Suba pela cidade e recolha o Selo de Hallownest indicado antes de entrar no Santuário das Almas.", "Wanderers_Journal.png", arrow: 0f),
            S("sr04_bench", "Seg. 04 • Comprar o banco do Santuário", "Se faltar Geo, venda uma relíquia a Lemm. Pague o banco e sente antes de enfrentar o Santuário.", "bench.png", arrow: 90f),
            S("sr04_twister", "Seg. 04 • Pegar Distorcedor de Magias", "Procure a abertura no teto antes da arena principal e recolha o amuleto.", "Spell_Twister.png", pb: "gotCharm_33", arrow: 45f),
            S("sr04_master", "Seg. 04 • Derrotar Mestre das Almas", "Derrote o chefe, obtenha Mergulho Desolador e recolha o baú e o selo na saída.", "Soul_Master.png", pi: "quakeLevel", min: 1, arrow: 270f),
            S("sr04_sell", "Seg. 04 • Vender todas as relíquias a Lemm", "Volte ao banco, equipe Distorcedor de Magias, venda as relíquias e faça save & quit. Proteja esse Geo.", "Wanderers_Journal.png", arrow: 180f),
            S("sr04_key", "Seg. 04 • Pegar uma Chave Simples", "Recolha a Chave Simples da Cidade; ela será usada para abrir os Esgotos Reais.", "Elegant_Key.png", pi: "simpleKeys", min: 1, arrow: 90f),
            S("sr04_stag", "Seg. 04 • Liberar Armazéns da Cidade", "Abra a estação, toque o sino e viaje de Último Besouro para a Encruzilhada.", "LastStag.png", pb: "openedRuins1", arrow: 90f),

            // Segmento 5 — Compras e entrada no Pico
            S("sr05_gruz", "Seg. 05 • Derrotar Mãe Mosca", "A partir do banco da Encruzilhada, derrote a Mãe Mosca e abra o caminho até Sly e Salubra.", "gruz_mother.png", pb: "killedBigFly", target: "Crossroads_04", arrow: 180f),
            S("sr05_sly", "Seg. 05 • Resgatar Sly", "Entre na casa da vila abandonada e fale com Sly até ele voltar para Dirtmouth.", "Sly_Basement.png", pb: "slyRescued", target: "Room_shop", arrow: 90f),
            S("sr05_steady", "Seg. 05 • Comprar Corpo Firme", "Compre Corpo Firme de Salubra.", "Salubra.png", pb: "gotCharm_14", arrow: 90f),
            S("sr05_shaman", "Seg. 05 • Comprar Pedra do Xamã", "Compre Pedra do Xamã e priorize-a junto de Distorcedor de Magias.", "Salubra.png", pb: "gotCharm_19", arrow: 90f),
            S("sr05_notch", "Seg. 05 • Comprar um encaixe de amuleto", "Com os cinco amuletos exigidos, compre o primeiro encaixe de Salubra e faça save & quit.", "Salubra.png", pi: "charmSlots", min: 4, arrow: 90f),
            S("sr05_lantern", "Seg. 05 • Comprar Lanterna de Lumélula", "Equipe Pedra do Xamã e Distorcedor de Magias, vá a Dirtmouth e compre a lanterna de Sly por 1800 Geo.", "Lumafly_Lantern.png", pb: "hasLantern", arrow: 90f),
            S("sr05_peak", "Seg. 05 • Entrar no Pico de Cristal", "Faça save & quit, volte à Encruzilhada e use Mergulho Desolador na entrada das minas.", "Crystal_Heart.png", pb: "visitedMines", arrow: 0f),

            // Segmento 6 — Pico de Cristal
            S("sr06_heart", "Seg. 06 • Obter Coração de Cristal", "Atravesse o lado direito do Pico. O banco de segurança próximo é opcional.", "Crystal_Heart.png", pb: "hasSuperDash", arrow: 90f),
            S("sr06_dark", "Seg. 06 • Obter Escuridão Descente", "Siga ao Monte Cristalizado e melhore o mergulho. Abuse da invencibilidade desse feitiço nos chefes.", "Descending_Dark.png", pi: "quakeLevel", min: 2, arrow: 90f),
            S("sr06_dream", "Seg. 06 • Obter o Ferrão dos Sonhos", "Caia nos Campos de Descanso e conclua a sequência do memorial.", "Dream_Nail.png", pb: "hasDreamNail", arrow: 180f),
            S("sr06_stag", "Seg. 06 • Liberar a estação dos Campos", "Abra a estação dos Campos de Descanso e viaje para Armazéns da Cidade.", "LastStag.png", pb: "openedRestingGrounds", arrow: 90f),

            // Segmento 7 — Lágrima de Isma e Lurien
            S("sr07_waterways", "Seg. 07 • Abrir os Esgotos Reais", "Use a Chave Simples na tampa da avenida inferior e sente no banco dos Esgotos.", "crawlid.png", pb: "openedWaterwaysManhole", arrow: 180f),
            S("sr07_dung", "Seg. 07 • Derrotar Defensor do Esterco", "Entre com vida e alma cheias. Use Escuridão Descente quando ele estiver sob o chão.", "Dung_Defender.png", pb: "killedDungDefender", arrow: 90f),
            S("sr07_isma", "Seg. 07 • Obter Lágrima de Isma", "Acione a alavanca, atravesse o túnel de espinhos com Coração de Cristal e alcance o bosque de Isma.", "Ismas_Tear.png", pb: "hasAcidArmour", arrow: 90f),
            S("sr07_skip", "Seg. 07 • Fazer o pogo dos Sentinelas", "Tente o pogo no objeto de fundo sob a Torre do Vigia. Se preferir segurança, pegue Asas do Monarca e retorne.", "Monarch_Wings.png", optional: true, arrow: 0f),
            S("sr07_watchers", "Seg. 07 • Derrotar Cavaleiros Sentinelas", "Use o banco da torre, derrube o lustre pelo teto secreto e abuse de Escuridão Descente durante a luta.", null, pb: "killedBlackKnight", arrow: 90f),
            S("sr07_lurien", "Seg. 07 • Libertar Lurien", "Suba ao topo da torre, encerre o sonho do Vigia e pegue o baú de Geo durante a descida.", "Dream_Nail.png", pb: "lurienDefeated", arrow: 0f),
            S("sr07_kings", "Seg. 07 • Liberar Estação do Rei", "Abra a estação inferior da cidade e viaje de Último Besouro até Dirtmouth.", "LastStag.png", pb: "openedRuins2", arrow: 180f),

            // Segmento 8 — Monomon
            S("sr08_archives", "Seg. 08 • Chegar aos Arquivos da Professora", "Atravesse o Caminho Verde e o Cânion da Névoa. Use o banco de segurança no topo dos Arquivos.", "Cornifer.png", arrow: 180f),
            S("sr08_uumuu", "Seg. 08 • Derrotar Uumuu", "Jogue com calma, atraia Uumuu para perto da plataforma central e use o mergulho quando Quirrel abrir sua defesa.", null, pb: "killedMegaJellyfish", arrow: 180f),
            S("sr08_monomon", "Seg. 08 • Libertar Monomon", "Encontre Quirrel junto ao tanque e use o Ferrão dos Sonhos na Professora.", "Dream_Nail.png", pb: "monomonDefeated", arrow: 90f),
            S("sr08_gardens", "Seg. 08 • Seguir para os Jardins da Rainha", "Use novamente o banco dos Arquivos e siga à esquerda, atravessando o ácido até os Jardins.", "crawlid.png", arrow: 270f),

            // Segmento 9 — Herrah
            S("sr09_bench", "Seg. 09 • Comprar o banco antes do Ninho", "Atravesse os Jardins pela entrada de ácido, pague o banco antes do Ninho Profundo e sente.", "bench.png", arrow: 270f),
            S("sr09_herrah", "Seg. 09 • Libertar Herrah", "Entre na Toca da Besta pela Vila Distante, atravesse a armadilha e encerre o sonho de Herrah.", "Dream_Nail.png", pb: "hegemolDefeated", arrow: 0f),
            S("sr09_stag", "Seg. 09 • Liberar a estação da Vila Distante", "Pague a estação e viaje diretamente para Dirtmouth.", "LastStag.png", pb: "openedDeepnest", arrow: 90f),

            // Segmento 10 — Hollow Knight
            S("sr10_egg", "Seg. 10 • Entrar no Templo do Ovo Negro", "Com os três Sonhadores derrotados, atravesse a Encruzilhada e entre no templo. Use o banco somente se precisar curar.", "Dream_Nail.png", pb: "openedBlackEggDoor", arrow: 90f),
            S("sr10_hollow_knight", "Seg. 10 • Derrotar o Hollow Knight", "Administre a alma, cure durante os golpes autoinfligidos e use Escuridão Descente como principal fonte de dano.", null, pb: "killedHollowKnight", arrow: 90f)
        };

        private static RouteStep S(string id, string title, string hint, string icon,
            bool optional = false, string pb = null, string pi = null, int min = 0,
            string scene = null, string target = null, float arrow = 90f)
        {
            return new RouteStep
            {
                Id = id,
                Title = title,
                Hint = hint,
                Icon = icon,
                Optional = optional,
                RequiredPlayerBool = pb,
                RequiredPlayerInt = pi,
                RequiredMinimum = min,
                RequiredScene = scene,
                TargetScene = target,
                ArrowDegrees = arrow
            };
        }
    }
}
