using System.Collections.Generic;
using System.Linq;

namespace HallownestWayfinder
{
    public static class RouteDefinition
    {
        public const string Name = "Rota 112%";

        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            Step("fury", "Pegar o amuleto Fúria dos Caídos", "Na Passagem do Rei, antes de entrar em Dirtmouth, desça pela abertura com espinhos. Atravesse usando ataques para baixo nos espinhos e alcance o baú à direita.", "Fury_of_the_Fallen.png", 135f, playerBool: "gotCharm_6", targetScene: "Tutorial_01"),
            Step("dirtmouth", "Ir para Dirtmouth", "Volte ao caminho principal da Passagem do Rei, siga sempre à direita e atravesse o grande portão para chegar a Dirtmouth.", "Elderbug.png", 90f, scene: "Town"),
            Step("crossroads", "Descer para a Encruzilhada Esquecida", "Em Dirtmouth, caminhe à direita do banco e pule no poço aberto.", "crawlid.png", 180f, scene: "Crossroads_01"),
            Step("crossroads_map", "Comprar o mapa com Cornifer", "Do fundo do poço, desça e siga para a esquerda. Procure os papéis no chão e acompanhe o som de Cornifer.", "Cornifer.png", 225f, optional: true, playerBool: "mapCrossroads", targetScene: "Crossroads_33"),
            Step("grub_1", "Salvar a primeira larva", "Da sala de Cornifer, saia pela esquerda. Continue para a esquerda e procure a jarra de vidro da larva; quebre-a com o ferrão.", "grub.png", 270f, playerInt: "grubsCollected", minimum: 1, targetScene: "Crossroads_35"),
            Step("crossroads_station", "Desbloquear a primeira estação", "Volte para a sala central alta da Encruzilhada. Desça até a parte inferior e siga pela saída da esquerda até a Estação da Encruzilhada Esquecida; pague 50 Geo e toque o sino.", "LastStag.png", 225f, playerBool: "openedCrossroads", targetScene: "Crossroads_47"),
            Step("grub_2", "Salvar a segunda larva", "Saia da estação pela direita. Na sala seguinte, suba e ataque a parede falsa do lado esquerdo para alcançar a larva.", "grub.png", 0f, playerInt: "grubsCollected", minimum: 2, targetScene: "Crossroads_03"),
            Step("grub_3", "Salvar a terceira larva", "Saia da entrada do Pico de Cristal pela esquerda. Suba pela sala vertical e pegue a saída da direita; siga até encontrar a larva.", "grub.png", 45f, playerInt: "grubsCollected", minimum: 3, targetScene: "Crossroads_48"),
            Step("grub_4", "Salvar a quarta larva", "Retorne à sala ao lado da parede falsa da segunda larva. Entre pela passagem oposta, avance para a direita e depois desça até a larva.", "grub.png", 135f, playerInt: "grubsCollected", minimum: 4, targetScene: "Crossroads_31"),
            Step("gruz_mother", "Derrotar a Mãe Mosca", "Saia da sala da quarta larva e continue descendo. Entre na arena grande e derrote a Mãe Mosca.", "gruz_mother.png", 180f, playerBool: "killedBigFly", targetScene: "Crossroads_04"),
            Step("sly", "Salvar Sly", "Após a Mãe Mosca, siga pela saída à direita e entre na pequena casa da vila abandonada. Fale com Sly até ele despertar.", "Sly_Basement.png", 90f, playerBool: "slyRescued", targetScene: "Room_shop"),
            Step("false_knight", "Derrotar o Falso Cavaleiro", "Volte à Estação da Encruzilhada Esquecida. Na sala vertical à direita da estação, suba e siga à esquerda até a arena do Falso Cavaleiro.", "False_Knight.png", 315f, playerBool: "killedFalseKnight", targetScene: "Crossroads_10"),
            Step("vengeful_spirit", "Pegar o Espírito Vingativo", "Depois da luta, saia pela esquerda e siga o Xamã dos Caracóis até o Monte Ancestral. Complete o desafio e receba o feitiço.", "Vengeful_Spirit_Icon.png", 270f, playerBool: "hasSpell", targetScene: "Crossroads_ShamanTemple"),
            Step("soul_catcher", "Pegar o Apanhador de Almas", "No Monte Ancestral, derrote o Baldur Ancião usando o Espírito Vingativo. O amuleto está logo depois dele.", "Soul_Catcher.png", 90f, playerBool: "gotCharm_20", targetScene: "Crossroads_ShamanTemple"),
            Step("greenpath", "Ir para o Caminho Verde", "Use o Espírito Vingativo para abrir o caminho.", "Elder_Baldur.png", 270f, scene: "Fungus1_01"),
            Step("grub_5", "Salvar a quinta larva", "Continue pelo Caminho Verde até ver Hornet pela primeira vez. Na área seguinte, procure uma passagem secreta e quebre o cipó que bloqueia o caminho; depois desça e siga pela direita até a larva.", "grub.png", 135f, playerInt: "grubsCollected", minimum: 5, targetScene: "Fungus1_06"),
            Step("greenpath_map", "Comprar o mapa do Caminho Verde", "Volte ao caminho principal, procure os papéis espalhados pelo chão e siga o canto de Cornifer até o nicho onde ele vende o mapa.", "Cornifer.png", 270f, optional: true, playerBool: "mapGreenpath", targetScene: "Fungus1_06"),
            Step("hunters_journal", "Pegar o Diário do Caçador", "Da região da quinta larva, desça e siga para a direita até a cabana do Caçador. Entre e aceite o Diário do Caçador.", "Hunter.png", 135f, playerBool: "hasJournal", targetScene: "Fungus1_08"),
            Step("grub_6", "Salvar a sexta larva", "Saia da cabana do Caçador, volte para a esquerda e desça. Explore a parte inferior até alcançar a jarra da larva.", "grub.png", 225f, playerInt: "grubsCollected", minimum: 6, targetScene: "Fungus1_07"),
            Step("greenpath_bench", "Sentar no banco (50 Geo)", "Retorne ao caminho de Hornet e siga para oeste, subindo pelas plataformas. Pague 50 Geo para liberar o banco e sente-se.", "bench.png", 315f, playerBool: "atBench"),
            Step("zote", "Salvar Zote", "Continue subindo a oeste. Depois do Cavaleiro do Musgo, siga até ouvir Zote e derrote o Rei Vingança; em seguida, fale com Zote.", "Vengefly_King_Zote.png", 315f, playerBool: "zoteRescuedBuzzer", targetScene: "Fungus1_20_v02"),
            Step("grub_7", "Salvar a sétima larva", "Continue pela parte superior do Caminho Verde. Derrote o Cavaleiro do Musgo que guarda a jarra e quebre-a para libertar a larva.", "grub.png", 90f, playerInt: "grubsCollected", minimum: 7, targetScene: "Fungus1_21"),
            Step("greenpath_station", "Liberar a estação do Caminho Verde", "Desça seguindo as placas com o símbolo do Último Besouro. Entre na estação, pague 140 Geo e toque o sino.", "LastStag.png", 180f, playerBool: "openedGreenpath", targetScene: "Fungus1_16_alt"),
            Step("wanderers_journal", "Pegar o Diário do Viajante", "Saia da estação e retome o caminho para oeste em direção ao Lago de Unn. Vasculhe a passagem antes da arena de Hornet para encontrar o diário.", "Wanderers_Journal.png", 270f, playerInt: "trinket1", minimum: 1, targetScene: "Fungus1_22"),
            Step("hornet", "Derrotar Hornet e obter o dash", "Derrote Hornet e colete o Manto de Asa de Mariposa.", "Hornet.png", 270f, playerBool: "hasDash", targetScene: "Fungus1_04")
        }.Concat(GuideContinuation.Steps).ToList();

        private static RouteStep Step(string id, string title, string hint, string icon, float arrowDegrees = 0f,
            bool optional = false, string scene = null, string playerBool = null,
            string playerInt = null, int minimum = 0, string targetScene = null)
        {
            return new RouteStep
            {
                Id = id,
                Title = title,
                Hint = hint,
                Icon = icon,
                ArrowDegrees = arrowDegrees,
                Optional = optional,
                RequiredScene = scene,
                RequiredPlayerBool = playerBool,
                RequiredPlayerInt = playerInt,
                RequiredMinimum = minimum
                ,TargetScene = targetScene
            };
        }
    }
}

