using System.Collections.Generic;

namespace HallownestWayfinder
{
    /// <summary>
    /// Continuação pós-Hornet, adaptada e resumida do guia 112% de Almech Alfarion.
    /// Os textos são orientações próprias e curtas para uso dentro do HUD.
    /// </summary>
    public static class GuideContinuation
    {
        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            // Capítulo 3 — Ermos Fúngicos
            S("c03_dirtmouth", "Cap. 03 • Voltar a Dirtmouth", "Retorne à Estação do Caminho Verde usando o dash e viaje de Último Besouro até Dirtmouth.", "LastStag.png", optional: true, scene: "Town", targetScene: "Fungus1_16_alt", transport: "Use o Último Besouro para viajar até Dirtmouth", arrow: 90f),
            S("c03_compass", "Cap. 03 • Comprar pena e bússola", "Na loja de Iselda, compre a Pena e o amuleto Bússola Caprichosa. Se estiver sem mapas, priorize o Enxame de Colecionadores em Sly.", "Cornifer.png", optional: true, pb: "gotCharm_2", targetScene: "Room_mapper", arrow: 90f),
            S("c03_grub6", "Cap. 03 • Salvar a próxima larva", "Ao descer pelo poço, siga pela esquerda até chegar à sala vertical e entre na sala abaixo à direita. Use o dash para alcançar a larva.", "grub.png", pi: "grubsCollected", min: 8, targetScene: "Crossroads_05", arrow: 90f),
            S("c03_wastes", "Cap. 03 • Ir para os Ermos Fúngicos", "Na parte inferior da Encruzilhada, atravesse o vão com dash e desça pelos corredores de cogumelos.", "crawlid.png", pb: "visitedFungus", targetScene: "Fungus2_06", arrow: 180f),
            S("c03_ogres", "Cap. 03 • Derrotar os Cogumelos", "Desça até o fundo do primeiro poço e siga à esquerda. Derrote os dois cogumelos guerreiros para receber um encaixe de amuleto.", "Shrumal_Warrior.png", targetScene: "Fungus2_05", arrow: 270f),
            S("c03_map", "Cap. 03 • Comprar o mapa dos Ermos", "Passe pela Estação da Rainha e encontre Cornifer na passagem abaixo das piscinas de ácido.", "Cornifer.png", optional: true, pb: "mapFungalWastes", arrow: 225f),
            S("c03_claw", "Cap. 03 • Obter a Garra de Louva-a-Deus", "Atravesse os cogumelos elásticos até a Vila dos Louva-a-Deus, acione a alavanca inferior e pegue a Garra na área superior.", "Mantis_Claw.png", pb: "hasWalljump", arrow: 270f),

            // Capítulo 4 — Cidade das Lágrimas
            S("c04_grub7", "Cap. 04 • Salvar uma larva nos Ermos", "Use a Garra para subir pelos túneis com cogumelos elásticos a caminho da Estação da Rainha.", "grub.png", pi: "grubsCollected", min: 9, arrow: 0f),
            S("c04_queen_station", "Cap. 04 • Liberar a Estação da Rainha", "Entre na plataforma inferior esquerda da Estação da Rainha, pague a passagem e chame o Último Besouro.", "LastStag.png", pb: "openedFungalWastes", arrow: 270f),
            S("c04_lantern", "Cap. 04 • Comprar a Lanterna", "Viaje a Dirtmouth e compre de Sly a Lanterna de Lumélula por 1800 Geo; depois retorne à Estação da Rainha.", "Lumafly_Lantern.png", pb: "hasLantern", arrow: 90f),
            S("c04_grub8", "Cap. 04 • Salvar larva sobre o ácido", "Perto do atalho para a Vila dos Louva-a-Deus, atravesse o salão de ácido e cogumelos elásticos até a larva.", "grub.png", pi: "grubsCollected", min: 10, arrow: 270f),
            S("c04_spore", "Cap. 04 • Pegar Cogumelo com Esporos", "Abaixo da larva, cruze as piscinas de ácido e pegue o amuleto; abra a parede à esquerda para sair.", "Spore_Shroom.png", pb: "gotCharm_17", arrow: 180f),
            S("c04_deepnest_grub", "Cap. 04 • Fazer uma visita curta ao Ninho Profundo", "Caia pelo piso frágil, abrace a parede direita e liberte a larva verdadeira na sala dos imitadores.", "grub.png", pi: "grubsCollected", min: 11, arrow: 180f),
            S("c04_deepnest_map", "Cap. 04 • Comprar o mapa do Ninho Profundo", "Desça e siga à esquerda até ouvir Cornifer. Não se afaste da rota nesta visita inicial.", "Cornifer.png", optional: true, pb: "mapDeepnest", arrow: 225f),
            S("c04_city", "Cap. 04 • Entrar na Cidade das Lágrimas", "Retorne aos Ermos, siga o caminho de Hornet, atravesse os espinhos e use o Brasão da Cidade na grande estátua.", "crawlid.png", pb: "visitedRuins", arrow: 90f),
            S("c04_nail1", "Cap. 04 • Afiar o ferrão", "Na parte baixa da cidade, encontre o Ferreiro à esquerda e compre a primeira melhoria por 250 Geo.", "Nailsmith.png", optional: true, pi: "nailSmithUpgrades", min: 1, arrow: 270f),
            S("c04_lemm", "Cap. 04 • Conhecer Lemm", "Suba pelas plataformas centrais, abra o edifício à direita e venda os seus primeiros artefatos para Lemm.", "Wanderers_Journal.png", arrow: 90f),
            S("c04_grub10", "Cap. 04 • Salvar a larva da cidade", "Suba pelos elevadores acima de Lemm e procure a sala lateral à direita.", "grub.png", pi: "grubsCollected", min: 12, arrow: 45f),
            S("c04_city_map", "Cap. 04 • Comprar o mapa da Cidade", "Abra o atalho até o banco da passarela elevada e compre o mapa com Cornifer.", "Cornifer.png", optional: true, pb: "mapCity", arrow: 270f),
            S("c04_spell_twister", "Cap. 04 • Pegar Distorcedor de Magias", "No Santuário das Almas, procure a abertura escura no teto antes da arena principal.", "Spell_Twister.png", pb: "gotCharm_33", arrow: 45f),
            S("c04_soul_master", "Cap. 04 • Derrotar Mestre das Almas", "Abra os atalhos do Santuário e avance pela porta esquerda do salão superior. Vencendo, receba Mergulho Desolador.", "Soul_Master.png", pi: "quakeLevel", min: 1, arrow: 270f),
            S("c04_grub11", "Cap. 04 • Salvar larva no Santuário", "Ao quebrar os pisos após o Mestre das Almas, pare na borda e procure a larva antes de cair ao fundo.", "grub.png", pi: "grubsCollected", min: 13, arrow: 180f),
            S("c04_storerooms", "Cap. 04 • Liberar Armazéns da Cidade", "Atravesse a passarela, pegue a Chave Simples e derrube o grande bloco para abrir a estação.", "LastStag.png", pb: "openedRuins1", arrow: 90f),

            // Capítulos 5–7 — Pico de Cristal, sonhos e Alma Sombria
            S("c05_salubra", "Cap. 05 • Comprar encaixe com Salubra", "Viaje à Encruzilhada, siga além da antiga arena da Mãe Mosca e compre o primeiro encaixe disponível.", "Salubra.png", arrow: 90f),
            S("c05_peak", "Cap. 05 • Entrar no Pico de Cristal", "Pegue o elevador acima de Myla e use Mergulho Desolador no chão instável para entrar nas minas.", "Elder_Baldur.png", pb: "visitedMines", arrow: 0f),
            S("c05_grub12", "Cap. 05 • Salvar larva entre esteiras", "Suba pelas esteiras verticais, atravesse os espinhos com dash e acione a alavanca após a larva.", "grub.png", pi: "grubsCollected", min: 14, arrow: 0f),
            S("c05_peak_map", "Cap. 05 • Comprar o mapa do Pico", "Suba pelos corredores de lasers até encontrar Cornifer.", "Cornifer.png", optional: true, pb: "mapMines", arrow: 0f),
            S("c05_shop_key", "Cap. 05 • Pegar a Chave do Lojista", "Continue subindo pelo lado direito das minas e pegue a chave no topo do poço.", "Shopkeepers_Key.png", pb: "hasSlykey", arrow: 45f),
            S("c05_heart", "Cap. 05 • Obter Coração de Cristal", "Atravesse o grande desafio de lasers no lado direito do Pico e examine o autômato quebrado.", "Crystal_Heart.png", pb: "hasSuperDash", arrow: 90f),
            S("c05_grubs", "Cap. 05 • Salvar as larvas do Pico", "Use o super dash para alcançar as larvas atrás do abismo, dos pistões e nas salas inferiores.", "grub.png", pi: "grubsCollected", min: 18, arrow: 270f),
            S("c05_dark", "Cap. 05 • Obter Escuridão Descente", "Atravesse a sala escura até o Monte Cristalizado, liberte o Xamã preso e melhore o mergulho.", "Descending_Dark.png", pi: "quakeLevel", min: 2, arrow: 90f),
            S("c06_dream_nail", "Cap. 06 • Obter o Ferrão dos Sonhos", "Caia do Monte Cristalizado aos Campos de Descanso e complete a sequência do memorial.", "Dream_Nail.png", pb: "hasDreamNail", arrow: 180f),
            S("c06_dreamshield", "Cap. 06 • Pegar Escudo dos Sonhos", "Explore a passagem abaixo da casa da Vidente antes de liberar a estação.", "Dreamshield.png", pb: "gotCharm_38", arrow: 180f),
            S("c06_station", "Cap. 06 • Liberar Campos de Descanso", "Atravesse o poço principal e acione a alavanca da estação.", "LastStag.png", pb: "openedRestingGrounds", arrow: 90f),
            S("c07_elegant", "Cap. 07 • Comprar a Chave Elegante", "Entregue a Chave do Lojista a Sly, compre a Chave Elegante e adquira o mapa dos Campos com Iselda.", "Elegant_Key.png", arrow: 90f),
            S("c07_shade_soul", "Cap. 07 • Obter Alma Sombria", "Retorne ao Santuário das Almas, abra a porta elegante, derrote o Guerreiro das Almas e liberte o Xamã.", "Vengeful_Spirit_Icon.png", pi: "fireballLevel", min: 2, arrow: 0f),
            S("c07_waterways", "Cap. 07 • Abrir os Esgotos Reais", "Desça à avenida inferior da cidade e use uma Chave Simples na tampa próxima ao elevador.", "crawlid.png", pb: "openedWaterwaysManhole", arrow: 180f),

            // Capítulos 8–9 — Esgotos e Bacia Antiga
            S("c08_grub17", "Cap. 08 • Salvar larva nos Esgotos", "A partir do banco inferior, suba e atravesse a parede quebrável à esquerda.", "grub.png", pi: "grubsCollected", min: 19, arrow: 270f),
            S("c08_map", "Cap. 08 • Comprar o mapa dos Esgotos", "Vença a sala de Hwurmps e encontre Cornifer no canto superior esquerdo.", "Cornifer.png", optional: true, pb: "mapWaterways", arrow: 315f),
            S("c08_dung", "Cap. 08 • Derrotar Defensor do Esterco", "Siga os túneis marrons após o banco e derrote Ogrim para receber o Brasão do Defensor.", "Dung_Defender.png", pb: "killedDungDefender", arrow: 90f),
            S("c08_isma", "Cap. 08 • Obter Lágrima de Isma", "Use o Coração de Cristal pelo túnel de espinhos, vença a emboscada de guardas e alcance o bosque de Isma.", "Ismas_Tear.png", pb: "hasAcidArmour", arrow: 90f),
            S("c09_grub18", "Cap. 09 • Salvar larva após Isma", "Nade pelo ácido à direita e suba pela parede até a larva.", "grub.png", pi: "grubsCollected", min: 20, arrow: 0f),
            S("c09_ore1", "Cap. 09 • Pegar Minério Pálido da Bacia", "Desça à Bacia Antiga e explore a área dos Mawleks no canto inferior esquerdo.", "Pale_Ore.png", pi: "ore", min: 1, arrow: 225f),
            S("c09_grub19", "Cap. 09 • Salvar larva na Bacia", "Use Escuridão Descente no piso solto próximo aos Mawleks menores.", "grub.png", pi: "grubsCollected", min: 21, arrow: 180f),
            S("c09_map", "Cap. 09 • Comprar o mapa da Bacia", "Desça pelas alcovas à direita da fonte e encontre Cornifer.", "Cornifer.png", optional: true, pb: "mapAbyss", arrow: 180f),
            S("c09_broken", "Cap. 09 • Derrotar Receptáculo Quebrado", "Atravesse o abismo de espinhos no lado esquerdo e siga até a arena.", "Broken_Vessel.png", pb: "killedInfectedKnight", arrow: 270f),
            S("c09_wings", "Cap. 09 • Obter Asas do Monarca", "Após a luta, siga à esquerda e para baixo até a antiga carcaça para receber o pulo duplo.", "Monarch_Wings.png", pb: "hasDoubleJump", arrow: 225f),
            S("c09_lost_kin", "Cap. 09 • Derrotar Parente Perdido", "Use o Ferrão dos Sonhos no corpo do Receptáculo Quebrado e vença sua versão onírica.", "Lost_Kin.png", pb: "infectedKnightDreamDefeated", arrow: 90f),
            S("c09_hidden_station", "Cap. 09 • Liberar Estação Oculta", "Use o pulo duplo no corredor sob o bonde, quebre a parede ao extremo direito e pague a estação.", "LastStag.png", pb: "openedHiddenStation", arrow: 90f),

            // Capítulos 10–13 — Grimm, montanha e segredos do Caminho Verde
            S("c10_ritual", "Cap. 10 • Acender a Chama do Pesadelo", "Do Caminho Verde, suba aos Penhascos Uivantes, encontre o grande cadáver oculto, use o Ferrão dos Sonhos e acenda a tocha.", null, pb: "nightmareLanternLit", arrow: 315f),
            S("c10_cliffs_map", "Cap. 10 • Comprar mapa dos Penhascos", "Retorne à face do penhasco e siga o canto de Cornifer.", "Cornifer.png", optional: true, pb: "mapCliffs", arrow: 0f),
            S("c10_joni", "Cap. 10 • Pegar Bênção de Joni", "Atravesse a sala escura seguindo as flores azuis até o repouso de Joni.", "Jonis_Blessing.png", pb: "gotCharm_27", arrow: 90f),
            S("c10_cyclone", "Cap. 10 • Aprender Corte Ciclone", "Suba ao topo dos penhascos e encontre a cabana de Mato na fenda sob a borda.", "Cyclone_Slash.png", pb: "hasCyclone", arrow: 90f),
            S("c10_gorb", "Cap. 10 • Derrotar Gorb", "Continue ao topo esquerdo e desafie o espírito no monumento.", "Gorb.png", pb: "aladarSlugDefeated", arrow: 315f),
            S("c10_stag_nest", "Cap. 10 • Visitar o Ninho dos Besouros", "Use um Vengemosca para alcançar a borda à esquerda de Gorb e pegue o fragmento de receptáculo dentro do ninho.", "LastStag.png", pb: "openedStagNest", arrow: 270f),
            S("c11_grimmchild", "Cap. 11 • Receber Grimmchild", "Volte a Dirtmouth, entre na tenda principal e fale com Mestre da Trupe Grimm.", "Grimmchild.png", pb: "gotCharm_40", arrow: 90f),
            S("c11_failed", "Cap. 11 • Derrotar Campeão Fracassado", "Na Encruzilhada Infectada, quebre a parede acima da antiga arena do Falso Cavaleiro e entre no sonho do corpo.", "Failed_Champion.png", pb: "falseKnightDreamDefeated", arrow: 315f),
            S("c11_xero", "Cap. 11 • Derrotar Xero", "Nos Campos de Descanso, desafie o espírito vermelho sobre a plataforma onde você caiu inicialmente.", "Xero.png", pb: "xeroDefeated", arrow: 270f),
            S("c11_dreamgate", "Cap. 11 • Desbloquear Portal dos Sonhos", "Entregue essência à Vidente até receber Minério Pálido, Amuleto do Portador e Portal dos Sonhos.", "Dreamgate.png", pb: "hasDreamGate", arrow: 90f),
            S("c12_deep_focus", "Cap. 12 • Pegar Foco Profundo", "Retorne ao Pico por Dirtmouth e use o Coração de Cristal através dos lasers até a geoda escondida.", "Deep_Focus.png", pb: "gotCharm_34", arrow: 90f),
            S("c12_guardians", "Cap. 12 • Derrotar Guardiões de Cristal", "Vença o Guardião no banco e depois suba pela parede direita para enfrentar sua versão enfurecida.", "Crystal_Guardian.png", pb: "defeatedMegaBeamMiner2", arrow: 0f),
            S("c12_ore", "Cap. 12 • Pegar Minério Pálido da Coroa", "Suba até a Coroa de Hallownest e pegue o minério aos pés da estátua.", "Pale_Ore.png", pi: "ore", min: 3, arrow: 315f),
            S("c13_sheo", "Cap. 13 • Aprender Grande Corte", "Na parte baixa do Caminho Verde, atravesse Durandas e espinhos até a casa de Sheo.", "Great_Slash.png", pb: "hasUpwardSlash", arrow: 270f),
            S("c13_thorns", "Cap. 13 • Pegar Espinhos da Agonia", "Atravesse as Lumélulas e os espinhos no corredor à direita da estação.", null, pb: "gotCharm_12", arrow: 90f),
            S("c13_grubs", "Cap. 13 • Vasculhar o Caminho Verde", "Libere as duas larvas dos corredores inferiores e recolha o fragmento de receptáculo escondido.", "grub.png", pi: "grubsCollected", min: 27, arrow: 270f),
            S("c13_wraiths", "Cap. 13 • Obter Espectros Uivantes", "Desça ao Cânion da Névoa, entre no Monte Verdejante e alcance o Xamã morto após a emboscada.", null, pi: "screamLevel", min: 1, arrow: 270f),
            S("c13_noeyes", "Cap. 13 • Derrotar Sem Olhos", "Entre no Santuário de Pedra, atravesse a escuridão e desafie o espírito.", null, pb: "noEyesDefeated", arrow: 90f),

            // Capítulos 14–18 — Borda do reino, Abismo e Sonhadores
            S("c14_souleater", "Cap. 14 • Pegar Devorador de Almas", "Nos túneis sob os Campos de Descanso, quebre as paredes e o teto ocultos próximos ao sarcófago.", null, pb: "gotCharm_21", arrow: 90f),
            S("c15_kings_station", "Cap. 15 • Liberar Estação do Rei", "Desça pelos elevadores orientais da cidade e pague a estação inferior.", "LastStag.png", pb: "openedRuins2", arrow: 180f),
            S("c15_edge_map", "Cap. 15 • Comprar mapa da Borda do Reino", "Saia pela estação quebrada, entre na Borda e desça ao tubo onde Cornifer canta.", "Cornifer.png", optional: true, pb: "mapOutskirts", arrow: 180f),
            S("c15_hornet2", "Cap. 15 • Derrotar Hornet Sentinela", "Atravesse a Borda pelo caminho superior e siga Hornet até a arena.", "Hornet.png", pb: "hornetOutskirtsDefeated", arrow: 90f),
            S("c15_brand", "Cap. 15 • Obter Marca do Rei", "Após Hornet, atravesse a Carcaça Abandonada e examine o símbolo no extremo esquerdo.", null, pb: "hasKingsBrand", arrow: 270f),
            S("c16_abyss", "Cap. 16 • Entrar no Abismo", "Viaje à Estação Oculta, desça pela Bacia e abra o portão com a Marca do Rei.", "crawlid.png", pb: "visitedAbyss", arrow: 180f),
            S("c16_shriek", "Cap. 16 • Obter Grito do Abismo", "No fundo esquerdo do Abismo, use Espectros Uivantes no centro da sala de rostos.", null, pi: "screamLevel", min: 2, arrow: 270f),
            S("c16_cloak", "Cap. 16 • Obter Manto Sombrio", "Acenda o farol no lado direito, atravesse o mar de Vazio e banhe-se na fonte escura.", null, pb: "hasShadowDash", arrow: 90f),
            S("c17_grimm", "Cap. 17 • Derrotar Mestre Grimm", "Retorne à tenda em Dirtmouth com Grimmchild equipado e vença Mestre da Trupe Grimm.", null, pb: "killedGrimm", arrow: 90f),
            S("c17_nail", "Cap. 17 • Melhorar o ferrão para Espiralado", "Venda relíquias a Lemm e leve os minérios ao Ferreiro na parte baixa da cidade.", null, pi: "nailSmithUpgrades", min: 3, arrow: 270f),
            S("c17_fluke", "Cap. 17 • Derrotar Flukemarm", "Nos Esgotos, mergulhe pelo piso abaixo do banco e encontre o caminho oculto à esquerda.", null, pb: "killedFlukeMother", arrow: 225f),
            S("c17_dashmaster", "Cap. 17 • Pegar Mestre da Esquiva", "Entre nos Ermos pela passagem dos Esgotos e pegue o amuleto aos pés da estátua.", null, pb: "gotCharm_31", arrow: 270f),
            S("c17_bretta", "Cap. 17 • Salvar Bretta", "Atravesse os corredores estreitos de espinhos à esquerda da estátua.", null, optional: true, pb: "brettaRescued", arrow: 270f),
            S("c17_lords", "Cap. 17 • Derrotar Lordes Louva-a-Deus", "Abra as alavancas da arena sob a Vila e desafie os três Lordes.", null, pb: "defeatedMantisLords", arrow: 180f),
            S("c17_pride", "Cap. 17 • Pegar Marca de Orgulho", "Após a vitória, entre na sala de tesouros recém-aberta à direita.", null, pb: "gotCharm_13", arrow: 90f),
            S("c18_mawlek", "Cap. 18 • Derrotar Mawlek Incubador", "Na Encruzilhada Infectada, quebre a porta sob a saída do Caminho Verde e atravesse os espinhos.", null, pb: "killedMawlek", arrow: 270f),
            S("c18_notch", "Cap. 18 • Pegar encaixe no Cânion", "Desça ao Cânion da Névoa e atravesse a sala de bolhas explosivas.", null, arrow: 270f),
            S("c18_fragile", "Cap. 18 • Comprar amuletos frágeis", "Equipe Brasão do Defensor para desconto e compre Coração, Ganância e Força com Come-Pernas.", null, pb: "gotCharm_25", arrow: 90f),
            S("c18_hu", "Cap. 18 • Derrotar Ancião Hu", "Siga ao leste dos Ermos Fúngicos e desafie o espírito no memorial.", null, pb: "elderHuDefeated", arrow: 90f),
            S("c18_fog_map", "Cap. 18 • Comprar mapa do Cânion", "Passe acima do portão sombrio no corredor oeste para alcançar Cornifer.", "Cornifer.png", optional: true, pb: "mapFogCanyon", arrow: 270f),
            S("c18_uumuu", "Cap. 18 • Derrotar Uumuu", "Entre nos Arquivos da Professora e desça até a arena; aguarde Quirrel abrir a defesa do chefe.", null, pb: "killedMegaJellyfish", arrow: 180f),
            S("c18_monomon", "Cap. 18 • Libertar Monomon", "Depois de Uumuu, encontre Quirrel junto ao tanque e use o Ferrão dos Sonhos em Monomon.", null, pb: "monomonDefeated", arrow: 90f),

            // Capítulos 19–23 — Jardins, Ninho Profundo, Colmeia e Vigia
            S("c19_gardens_map", "Cap. 19 • Comprar mapa dos Jardins", "Passe da Estação da Rainha aos Jardins, vença a emboscada de mantis e siga as placas até Cornifer.", "Cornifer.png", optional: true, pb: "mapRoyalGardens", arrow: 270f),
            S("c19_love_key", "Cap. 19 • Pegar Chave do Amor", "Nos Jardins, desça ao salão do grande cadáver e derrote os Mantis Traidores.", null, pb: "hasLoveKey", arrow: 180f),
            S("c19_marmu", "Cap. 19 • Derrotar Marmu", "Siga pelo lado direito dos Jardins até o túmulo do guerreiro.", null, pb: "mumCaterpillarDefeated", arrow: 90f),
            S("c19_station", "Cap. 19 • Liberar Estação dos Jardins", "Continue à direita após Marmu, pague a estação e abra o atalho.", "LastStag.png", pb: "openedRoyalGardens", arrow: 90f),
            S("c19_traitor", "Cap. 19 • Derrotar Lorde Traidor", "Atravesse as plataformas de espinhos acima da estação e use o Manto Sombrio na arena.", null, pb: "killedTraitorLord", arrow: 270f),
            S("c19_queen", "Cap. 19 • Receber metade da Alma do Rei", "Após o Lorde Traidor, entre no santuário da Dama Branca e fale com ela.", null, pb: "gotQueenFragment", arrow: 270f),
            S("c20_herrah", "Cap. 20 • Libertar Herrah", "Entre na Toca da Besta na Vila Distante, escape da armadilha, suba ao topo e encerre o sonho de Herrah.", null, pb: "hegemolDefeated", arrow: 0f),
            S("c20_weaver", "Cap. 20 • Pegar Canção das Tecelãs", "No Ninho Profundo, rompa a parede atrás do Devoto Espreitador e atravesse o piso de espinhos.", null, pb: "gotCharm_39", arrow: 270f),
            S("c20_galien", "Cap. 20 • Derrotar Galien", "Encontre o guerreiro caído próximo aos cogumelos brancos e desafie o seu espírito.", null, pb: "galienDefeated", arrow: 270f),
            S("c20_pass", "Cap. 20 • Pegar Passe do Bonde", "Suba ao Bonde Fracassado e explore o vagão destruído no extremo esquerdo.", null, pb: "hasTramPass", arrow: 270f),
            S("c20_zote", "Cap. 20 • Salvar Zote no Ninho", "Após o Bonde Fracassado, desça pela saída esquerda e liberte Zote das teias.", "Vengefly_King_Zote.png", optional: true, pb: "zoteRescuedDeepnest", arrow: 180f),
            S("c20_nosk", "Cap. 20 • Derrotar Nosk", "A partir das Termas do Ninho, atravesse a parede rachada e siga a figura familiar até a arena.", null, pb: "killedMimicSpider", arrow: 270f),
            S("c20_sharp", "Cap. 20 • Pegar Sombra Afiada", "Atravesse o portão sombrio na sala dos Garpedes e alcance o amuleto.", null, pb: "gotCharm_16", arrow: 90f),
            S("c21_hive", "Cap. 21 • Entrar na Colmeia", "Use o Passe do Bonde, viaje duas vezes à direita e quebre a parede acima dos insetos no túnel.", "crawlid.png", pb: "visitedHive", arrow: 90f),
            S("c21_knight", "Cap. 21 • Derrotar Cavaleiro da Colmeia", "Atravesse as grandes câmaras até o extremo direito e rompa as barreiras antes da arena.", null, pb: "killedHiveKnight", arrow: 90f),
            S("c21_hiveblood", "Cap. 21 • Pegar Sangue da Colmeia", "Após o chefe, passe sob a arena e alcance a câmara da rainha.", null, pb: "gotCharm_29", arrow: 90f),
            S("c22_markoth", "Cap. 22 • Derrotar Markoth", "Retorne à Borda do Reino, atravesse o portão sombrio inferior e desafie o guerreiro caído.", null, pb: "markothDefeated", arrow: 90f),
            S("c22_dashslash", "Cap. 22 • Aprender Corte Veloz", "Siga à direita pelos Grandes Saltadores e compre a técnica com Oro por 800 Geo.", null, pb: "hasDashSlash", arrow: 90f),
            S("c22_quickslash", "Cap. 22 • Pegar Corte Rápido", "Passe os Grandes Saltadores, suba na estrutura da parede direita e pegue o amuleto.", null, pb: "gotCharm_32", arrow: 270f),
            S("c23_collector", "Cap. 23 • Derrotar o Colecionador", "Entre na Torre do Amor com a chave e suba até a arena.", null, pb: "collectorDefeated", arrow: 0f),
            S("c23_grubs", "Cap. 23 • Libertar as larvas da torre", "Após o Colecionador, explore o salão superior, pegue o mapa e liberte as três larvas.", "grub.png", pi: "grubsCollected", min: 44, arrow: 0f),
            S("c23_final_grub", "Cap. 23 • Salvar a última larva", "Na Torre do Vigia, abra as alavancas e procure a sala acima da saída direita.", "grub.png", pi: "grubsCollected", min: 46, arrow: 0f),
            S("c23_watchers", "Cap. 23 • Derrotar Cavaleiros Sentinelas", "Quebre a corrente no teto para reduzir a luta e entre na arena superior da torre.", null, pb: "killedBlackKnight", arrow: 90f),
            S("c23_lurien", "Cap. 23 • Libertar Lurien", "Suba ao topo da torre após os Cavaleiros e encerre o sonho do Vigia.", null, pb: "lurienDefeated", arrow: 0f),

            // Capítulos 24–30 — conclusão 112%
            S("c24_white_defender", "Cap. 24 • Derrotar Defensor Branco", "Nos Esgotos, entre no sonho de Ogrim na câmara abaixo do elevador quebrado.", null, pb: "killedWhiteDefender", arrow: 180f),
            S("c24_colosseum", "Cap. 24 • Completar o Coliseu", "Suba à Arena dos Tolos, conclua as três provas e derrote o Domador de Deuses.", null, pb: "colosseumGoldCompleted", arrow: 0f),
            S("c25_zote", "Cap. 25 • Derrotar Príncipe Cinzento Zote", "Em Dirtmouth, entre no porão da casa de Bretta e use o Ferrão dos Sonhos na estátua.", null, optional: true, pb: "killedGreyPrince", arrow: 180f),
            S("c25_grubfather", "Cap. 25 • Receber recompensas das larvas", "Visite o Lar das Larvas na Encruzilhada e recolha todas as recompensas do Pai das Larvas.", "grub.png", pb: "gotCharm_35", arrow: 270f),
            S("c25_sly", "Cap. 25 • Receber Glória do Mestre", "Com as três artes aprendidas, visite o porão de Sly e depois compre os fragmentos e amuletos restantes.", "Sly_Basement.png", pb: "gotCharm_26", arrow: 90f),
            S("c25_awoken", "Cap. 25 • Despertar o Ferrão dos Sonhos", "Reúna 2400 de essência e entregue-a à Vidente até ela desaparecer.", null, pb: "dreamNailUpgraded", arrow: 90f),
            S("c26_flower", "Cap. 26 • Entregar a Flor Delicada", "Aceite a flor com a Mourner Cinzenta e atravesse Campos, Encruzilhada, Cânion e Jardins sem sofrer dano até o túmulo.", null, pb: "xunRewardGiven", arrow: 270f),
            S("c27_palace", "Cap. 27 • Entrar no Palácio Branco", "Na Bacia Antiga, use o Ferrão Desperto no molde real morto e atravesse o sonho até o portão.", null, pb: "visitedWhitePalace", arrow: 90f),
            S("c27_kingsoul", "Cap. 27 • Completar Alma do Rei", "Supere as três alas do Palácio Branco, alcance o trono e golpeie o corpo do Rei Pálido.", null, pb: "gotKingFragment", arrow: 0f),
            S("c28_lifeblood", "Cap. 28 • Pegar Núcleo de Sangue Vital", "Equipe vida azul e Alma do Rei, abra a porta selada no Abismo e pegue o amuleto antes de sair.", null, pb: "gotCharm_9", arrow: 180f),
            S("c28_void", "Cap. 28 • Transformar Alma do Rei em Coração Vazio", "No fundo do Abismo, quebre o piso, encontre o ovo negro e use o Ferrão dos Sonhos; depois suba pelo nascimento.", null, pb: "gotShadeCharm", arrow: 180f),
            S("c29_pure_nail", "Cap. 29 • Forjar Ferrão Puro", "Venda as últimas relíquias e entregue 4000 Geo e três Minérios Pálidos ao Ferreiro.", null, pi: "nailSmithUpgrades", min: 4, arrow: 270f),
            S("c29_unbreakable", "Cap. 29 • Melhorar amuletos frágeis", "Antes do fim da Trupe, entregue os amuletos a Divine e pague pelas versões inquebráveis.", null, optional: true, arrow: 270f),
            S("c29_grimm_end", "Cap. 29 • Encerrar a Trupe Grimm", "Escolha entre completar o Ritual contra Rei do Pesadelo Grimm ou destruir a lanterna com Brumm.", null, arrow: 90f),
            S("c30_godhome", "Cap. 30 • Abrir Lar dos Deuses", "Nos Esgotos, atravesse o caminho acima de Flukemarm até o Depósito de Lixo e abra o sarcófago com Chave Simples.", null, pb: "hasGodfinder", arrow: 270f),
            S("c30_pantheons", "Cap. 30 • Completar os Panteões", "No Lar dos Deuses, conclua os quatro primeiros Panteões para 112%. O Panteão de Hallownest permanece como desafio final opcional.", null, arrow: 90f)
        };

        private static RouteStep S(string id, string title, string hint, string icon = null,
            bool optional = false, string pb = null, string pi = null, int min = 0,
            float arrow = 90f, string scene = null, string targetScene = null, string transport = null)
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
                ArrowDegrees = arrow,
                TargetScene = targetScene,
                TransportInstruction = transport
            };
        }
    }
}

