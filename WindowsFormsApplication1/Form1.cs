using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string dificuldade, strUsuario = string.Empty, strComputador = string.Empty;
        bool cpuComeca = false, vezdaCpu = false;
        int contvitoriasCpu = 0, contvitoriasUsuario = 0, contVelhas = 0;
        Random rndSorteio = new Random(DateTime.Now.Millisecond);
        List<Button> listaCasasVazias = new List<Button>();
        List<Button> listaCasasVizinhas;
        Button penultimaSelecaoUsuario = null;
        Button ultimaSelecaoUsuario = null;

        List<Button> listaLinha1 = new List<Button>();
	    List<Button> listaLinha2 = new List<Button>();
	    List<Button> listaLinha3 = new List<Button>();
        

        public Form1()
        {
            InitializeComponent();
        }

        private void inicializar()
        // meu método de limpar tudo
        {
            listaLinha1.Add(btna1);
			listaLinha1.Add(btnb1);
			listaLinha1.Add(btnc1);
			
			listaLinha2.Add(btna2);
			listaLinha2.Add(btnb2);
			listaLinha2.Add(btnc2);
			
			listaLinha3.Add(btna3);
			listaLinha3.Add(btnb3);
			listaLinha3.Add(btnc3);

            comboBox1.SelectedValue = "Facil";

            int n = rndSorteio.Next(1,10);
            if (n % 2 == 0)
            {
                cpuComeca = true;
                vezdaCpu = true;
            }
            else
            {
                cpuComeca = false;
                vezdaCpu = false;
            }

            reiniciarJogo();
            exibirContagem();
        }

        private void exibirContagem()
        {
            txtUsuario.Text = contvitoriasUsuario.ToString();
            txtCpu.Text = contvitoriasCpu.ToString();
            txtVelha.Text = contVelhas.ToString();

        }

        private void reiniciarJogo()
        {
            penultimaSelecaoUsuario = null;
            ultimaSelecaoUsuario = null;

            dificuldade = Convert.ToString(comboBox1.SelectedItem);
            btna1.Text = string.Empty;
            btna2.Text = string.Empty;
            btna3.Text = string.Empty;
            btnb1.Text = string.Empty;
            btnb2.Text = string.Empty;
            btnb3.Text = string.Empty;
            btnc1.Text = string.Empty;
            btnc2.Text = string.Empty;
            btnc3.Text = string.Empty;

            btna1.ImageIndex = -1;
            btna2.ImageIndex = -1;
            btna3.ImageIndex = -1;
            btnb1.ImageIndex = -1;
            btnb2.ImageIndex = -1;
            btnb3.ImageIndex = -1;
            btnc1.ImageIndex = -1;
            btnc2.ImageIndex = -1;
            btnc3.ImageIndex = -1;

            /* reinicializa marcador das minhas casas vazias */
            listaCasasVazias.Clear();
            listaCasasVazias.Add(btna1);
            listaCasasVazias.Add(btna2);
            listaCasasVazias.Add(btna3);
            listaCasasVazias.Add(btnb1);
            listaCasasVazias.Add(btnb2);
            listaCasasVazias.Add(btnb3);
            listaCasasVazias.Add(btnc1);
            listaCasasVazias.Add(btnc2);
            listaCasasVazias.Add(btnc3);
            /* pronto */

            if (cpuComeca)
            {
                strComputador = "X";
                strUsuario = "O";
                CpuJoga();
            }
            else
            {
                strComputador = "O";
                strUsuario = "X";
            }

            cpuComeca = !cpuComeca;
        }



        private void jogoFacil()
        {
            bool marcada = false;
            Button casaSorteada = listaCasasVazias[rndSorteio.Next(listaCasasVazias.Count)];
            
            marcada = !marcaCasa(casaSorteada, strComputador);

            if (marcada)
            {
                jogoFacil();
            }
            else
            {
                vezdaCpu = false;
            }
        }

        private void jogoMedio()
        {

            Button casaSelecionada;
            bool marcada = false;


            // seleciona a casa a ser marcada ===========================================================

            // penultimaSelecaoUsuario = ultimaSelecaoUsuario;	
            // o usuário não marcou nenhuma casa
            if (ultimaSelecaoUsuario == null)
            {
                // seleciona randomicamente uma casa
                casaSelecionada = listaCasasVazias[rndSorteio.Next(listaCasasVazias.Count)];
            }
            else
            {

                // se  o usuário selecionou apenas uma casa até agora
                if (penultimaSelecaoUsuario == null)
                {
                    // tenta marcar uma casa próxima a dele

                    listaCasasVizinhas = listarCasasVizinhas(ultimaSelecaoUsuario);
                    // seleciona randomicamente uma casa vizinha
                    casaSelecionada = listaCasasVizinhas[rndSorteio.Next(listaCasasVizinhas.Count)];

                }
                else
                {

                    // obtém a casa prevista para o próximo passo do usuário
                    casaSelecionada = obterPrevisaoSelecaoUsuario(penultimaSelecaoUsuario, ultimaSelecaoUsuario);

                    // se nao tiver previsao de jogo, ou se a casa não estiver disponível para jogar
                    if (casaSelecionada == null || ((Button)casaSelecionada).Text != string.Empty)
                    {
                        // joga randomicamente
                        casaSelecionada = listaCasasVazias[rndSorteio.Next(listaCasasVazias.Count)];
                    }

                }

            }

            // ==========================================================================================


            // tenta marcar a casa sorteada			
            if (!marcaCasa(casaSelecionada, strComputador))
            {
                // se não conseguiu indica
                marcada = true;
            }


            // se a casa sorteada já estiver marcada

            if (marcada)
            {
                // seleciona novamente uma casa, até que encontre uma vazia
                jogoMedio();
            }
            else
            {
                // como consegui marcar, 
                // indica que não é a vez da cpu jogar
                vezdaCpu = false;
            }

        }




        private List<Button> listarCasasVizinhas(Button casa)
        {
            List<Button> lista = new List<Button>();


            if (btna1 == casa)
            {
                lista.Add(btnb1);
                lista.Add(btnb2);
                lista.Add(btnb2);
                return lista;
            }

            if (btna2 == casa)
            {
                lista.Add(btna1);
                lista.Add(btnb2);
                lista.Add(btna3);
                return lista;
            }

            if (btna3 == casa)
            {
                lista.Add(btna2);
                lista.Add(btnb2);
                lista.Add(btnb3);
                return lista;
            }

            if (btnb1 == casa)
            {
                lista.Add(btna1);
                lista.Add(btnb2);
                lista.Add(btnc1);
                return lista;
            }

            if (btnb2 == casa)
            {
                lista.Add(btna1);
                lista.Add(btnb1);
                lista.Add(btnc1);
                lista.Add(btnc2);
                lista.Add(btnc3);
                lista.Add(btnb3);
                lista.Add(btna3);
                lista.Add(btna2);
                return lista;
            }

            if (btnb3 == casa)
            {
                lista.Add(btna3);
                lista.Add(btnb2);
                lista.Add(btnc3);
                return lista;
            }

            if (btnc1 == casa)
            {
                lista.Add(btnb1);
                lista.Add(btnb2);
                lista.Add(btnc2);
                return lista;
            }

            if (btnc2 == casa)
            {
                lista.Add(btnc1);
                lista.Add(btnb2);
                lista.Add(btnc3);
                return lista;
            }

            if (btnc3 == casa)
            {
                lista.Add(btnc2);
                lista.Add(btnb2);
                lista.Add(btnb3);
                return lista;
            }

            return null;
        }


        /* Obtém a previsão de jogo do usuário baseada nas últimas 2 seleções */
        
        private Button obterPrevisaoSelecaoUsuario(Button pCasa, Button uCasa)
        {

            string spCasa = pCasa.Text;
            string suCasa = uCasa.Text;


            // col 1
            if (spCasa == btna1.Text && suCasa == btna2.Text)
            {
                return btna3;
            }
            if (spCasa == btna1.Text && suCasa == btna3.Text)
            {
                return btna2;
            }
            if (spCasa == btna2.Text && suCasa == btna3.Text)
            {
                return btna1;
            }

            // col 2
            if (spCasa == btnb1.Text && suCasa == btnb2.Text)
            {
                return btnb3;
            }
            if (spCasa == btnb1.Text && suCasa == btnb3.Text)
            {
                return btnb2;
            }
            if (spCasa == btnb2.Text && suCasa == btnb3.Text)
            {
                return btnb1;
            }

            // col 3
            if (spCasa == btnc1.Text && suCasa == btnc2.Text)
            {
                return btnc3;
            }
            if (spCasa == btnc1.Text && suCasa == btnc3.Text)
            {
                return btnc2;
            }
            if (spCasa == btnc2.Text && suCasa == btnc3.Text)
            {
                return btnc1;
            }


            // linha 1
            if (spCasa == btna1.Text && suCasa == btnb1.Text)
            {
                return btnc1;
            }
            if (spCasa == btna1.Text && suCasa == btnc1.Text)
            {
                return btnb1;
            }
            if (spCasa == btnb1.Text && suCasa == btnc1.Text)
            {
                return btna1;
            }

            // linha 2
            if (spCasa == btna2.Text && suCasa == btnb2.Text)
            {
                return btnc2;
            }
            if (spCasa == btna2.Text && suCasa == btnc2.Text)
            {
                return btnb2;
            }
            if (spCasa == btnb2.Text && suCasa == btnc2.Text)
            {
                return btna2;
            }

            // linha 3
            if (spCasa == btna3.Text && suCasa == btnb3.Text)
            {
                return btnc3;
            }
            if (spCasa == btna3.Text && suCasa == btnc3.Text)
            {
                return btnb3;
            }
            if (spCasa == btnb3.Text && suCasa == btnc3.Text)
            {
                return btna3;
            }

            // diagonal supesq infdir
            if (spCasa == btna1.Text && suCasa == btnb2.Text)
            {
                return btnc3;
            }
            if (spCasa == btna1.Text && suCasa == btnc3.Text)
            {
                return btnb2;
            }
            if (spCasa == btnb2.Text && suCasa == btnc3.Text)
            {
                return btna1;
            }


            // diagonal supesq infdir
            if (spCasa == btna3.Text && suCasa == btnb2.Text)
            {
                return btnc1;
            }
            if (spCasa == btna3.Text && suCasa == btnc1.Text)
            {
                return btnb2;
            }
            if (spCasa == btnb2.Text && suCasa == btnc1.Text)
            {
                return btna3;
            }

            return null;

        }

        private string checa_casa(int i, int j)
	{
		List<Button> aux = null;
	
		switch(j){
		case 1:
			aux = listaLinha1;
			break;
		case 2:
			aux = listaLinha2;
			break;
		case 3:
			aux = listaLinha3;
			break;
		}
	
		if (i>=1 && i <=3){
			return aux[i-1].Text;
		}
	
		return "";
	}
	
	    private void domina_casa(int i, int j, string caractere)
	    {
		
			List<Button> aux = null;
		
			switch(j){
			case 1:
				aux = listaLinha1;
				break;
			case 2:
				aux = listaLinha2;
				break;
			case 3:
				aux = listaLinha3;
				break;
			}
		
			if (i>=1 && i <=3){
				aux[i-1].Text = caractere;
                if (caractere.Equals("O"))
                {
                    aux[i-1].ImageIndex = 1;
                }
                else
                {
                    aux[i-1].ImageIndex = 0;
                }
			}
		
		if(caractere == strComputador){
			vezdaCpu = false;
		}
	}
		
		private int jogoDificil()
		{
		    string aux, aux1, aux2, aux3;
			int linha, coluna;
			string marcador;
		
			List<string> listaMarcadores = new List<string>();
		
			listaMarcadores.Add(strUsuario); //0 usuario
			listaMarcadores.Add(strComputador); //1 computador
			
			/* Level 1 AI */
			aux1 = checa_casa(2,2);
			if (aux1 == "")
			{
				domina_casa(2,2, strComputador);
				return 0; // retorno do metodo
		    }
		    
			for (linha = 2; linha >= 1; linha--)
			{
			
				marcador = listaMarcadores[linha-1];
			
				for (coluna = 1; coluna <= 3; coluna++)
				{
					// Horizontais
					aux1 = checa_casa(1,coluna);
					aux2 = checa_casa(2,coluna);
					aux3 = checa_casa(3,coluna);
					if ((aux1 == aux3) && (aux2 == "") && (aux1 == marcador))
					{
						aux = checa_casa(2,coluna);
						if (aux == "")
						{
		    				domina_casa(2,coluna, strComputador);
			    			return 0;
		        		}
		            }
					else if ((aux1 == aux2) && (aux3 == "") && (aux1 == marcador))
					{
		   				aux = checa_casa(3,coluna);
						if (aux == "")
						{
		                   domina_casa(3,coluna,strComputador);
						   return 0;
		       			}
		            }
					else if ((aux2 == aux3) && (aux1 == "") && (aux2 == marcador))
					{
		   				aux = checa_casa(1,coluna);
		   				if (aux == "")
		   				{
				 			domina_casa(1,coluna,strComputador);
							return 0;
		            	}
		            }
		
					// Verticais
					aux1 = checa_casa(coluna,1);
					aux2 = checa_casa(coluna,2);
					aux3 = checa_casa(coluna,3);
					if ((aux1 == aux3) && (aux2 == "") && (aux1 == marcador))
					{
		   				aux = checa_casa(coluna,2);
		   				if (aux == "")
		   				{
							domina_casa(coluna,2,strComputador);
							return 0;
		            	}
		            }
					else if ((aux1 == aux2) && (aux3 == "") && (aux1 == marcador))
					{
		   				aux = checa_casa(coluna,3);
		   				if (aux == "")
		   				{
							domina_casa(coluna,3,strComputador);
							return 0;
		            	}
		            }
		    		else if ((aux2 == aux3) && (aux1 == "") && (aux2 == marcador))
		    		{
		      			aux = checa_casa(coluna,1);
		      			if (aux == "")
		      			{
							domina_casa(coluna,1,strComputador);
							return 0;
		            	}
		            }
				}
			
				// Diagonal Principal
				aux1 = checa_casa(1,1);
				aux2 = checa_casa(2,2);
				aux3 = checa_casa(3,3);
			    if ((aux1 == aux3) && (aux2 == "") && (aux1 == marcador))
			    {
		     		aux = checa_casa(2,2);
		     		if (aux == "")
		     		{
						domina_casa(2,2,strComputador);
						return 0;
		        	}
		        }
				else if ((aux1 == aux2) && (aux3 == "") && (aux1 == marcador))
				{
		    		aux = checa_casa(3,3);
		    		if (aux == "")
		    		{
		    			domina_casa(3,3,strComputador);
		    			return 0;
		        	}
		        }
				else if ((aux2 == aux3) && (aux1 == "") && (aux2 == marcador))
				{
		  			aux = checa_casa(1,1);
		  			if (aux == "")
		  			{
						domina_casa(1,1,strComputador);
						return 0;
		        	}
		        }
				
				// Diagonal Secundaria
				aux1 = checa_casa(3,1);
				aux3 = checa_casa(1,3);
				if ((aux1 == aux3) && (aux2 == "") && (aux1 == marcador))
				{
		  			aux = checa_casa(2,2);
		  			if (aux == "")
		  			{
						domina_casa(2,2,strComputador);
						return 0;
		    		}	
		        }
				else if ((aux1 == aux2) && (aux3 == "") && (aux1 == marcador))
				{
		  			aux = checa_casa(1,3);
		  			if (aux == "")
		  			{
		    			domina_casa(1,3,strComputador);
		    			return 0;
		      		}
		        }
				else if ((aux2 == aux3) && (aux1 == "") && (aux2 == marcador))
				{
		  			aux = checa_casa(3,1);
		  			if (aux == "")
		  			{
						domina_casa(3,1,strComputador);
						return 0;
		            }
		        }
			}
				
			// Level 2 AI
			
			// Defesa
			aux1 = checa_casa(2,1);
			aux2 = checa_casa(1,3);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(1,1) == "")
				{
					domina_casa(1,1,strComputador);
					return 0;
				}
			}
		
			aux1 = checa_casa(2,1);
			aux2 = checa_casa(3,3);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(3,1) == "")
				{
					domina_casa(3,1,strComputador);
					return 0;
				}
			}
			
			aux1 = checa_casa(1,1);
			aux2 = checa_casa(2,3);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(1,3) == "")
				{
					domina_casa(1,3,strComputador);
					return 0;
				}
			}
		
			aux1 = checa_casa(3,1);
			aux2 = checa_casa(2,3);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(3,3) == "")
				{
					domina_casa(3,3,strComputador);
					return 0;
				}
			}
		
			aux1 = checa_casa(1,2);
			aux2 = checa_casa(3,1);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(1,1) == "")
				{
					domina_casa(1,1,strComputador);
					return 0;
				}
			}
		
			aux1 = checa_casa(1,2);
			aux2 = checa_casa(3,3);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(1,3) == "")
				{
					domina_casa(1,3,strComputador);
					return 0;
				}
			}
		
			aux1 = checa_casa(3,2);
			aux2 = checa_casa(1,1);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(3,1) == "")
				{
					domina_casa(3,1,strComputador);
					return 0;
				}
			}
		
			aux1 = checa_casa(3,2);
			aux2 = checa_casa(1,3);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if (checa_casa(3,3) == "")
				{
					domina_casa(3,1,strComputador);
					return 0;
				}
			}
		
			aux1 = checa_casa(1,1);
			aux2 = checa_casa(3,3);
			if ((aux1 == strUsuario) && (aux2 == strUsuario))
			{
		 		aux = checa_casa(2,1);
		 		if (aux == "")
		 		{
					domina_casa(2,1,strComputador);
					return 0;
		        }
		    }
		   
		    aux1 = checa_casa(3,1);
		    aux2 = checa_casa(1,3);
		    if ((aux1 == strUsuario) && (aux2 == strUsuario))
		    {
		    	aux = checa_casa(2,1);
		    	if (aux == "")
		    	{
		    		domina_casa(2,1,strComputador);
		    		return 0;
		        }
		    }
		    
		    aux1 = checa_casa(2,1);
		    aux2 = checa_casa(3,2);
		    aux3 = checa_casa(1,2);
		    if ((aux1 == strUsuario) && (aux2 == strUsuario))
		    {
		    	aux = checa_casa(3,1);
		    	if (aux == "")
		    	{
		        	domina_casa(3,1,strComputador);
		        	return 0;
		     	}
		    }
		    if ((aux1 == strUsuario) && (aux3 == strUsuario))
		    {
		    	aux = checa_casa(1,1);
		    	if (aux == "")
		    	{
		            domina_casa(1,1,strComputador);
		            return 0;
		        }
		    }
		    if ((aux3 == strUsuario) && (aux2 == strUsuario))
		    {
		    	aux = checa_casa(1,1);
		    	if (aux == "")
		    	{
		            domina_casa(1,1,strComputador);
		            return 0;
		        }
		    }
		    
		    aux1 = checa_casa(2,3);
		    if ((aux1 == strUsuario) && (aux2 == strUsuario))
		    {
		    	aux = checa_casa(3,3);
		    	if (aux == "")
		    	{
		            domina_casa(3,3,strComputador);
		            return 0;
		        }
		    }
		    if ((aux1 == strUsuario) && (aux3 == strUsuario))
		    {
		    	aux = checa_casa(1,3);
		    	if (aux == "")
		    	{
		            domina_casa(1,3,strComputador);
		            return 0;
		        }
		    }
		    
		    aux1 = checa_casa(1,1);
		    aux2 = checa_casa(2,2);
		    aux3 = checa_casa(3,3);
		    if ((aux1 == strComputador) && (aux2 == strUsuario) && (aux3 == strUsuario))
		    {
		    	aux = checa_casa(3,1);
		    	if (aux == "")
		    	{
		            domina_casa(3,1,strComputador);
		            return 0;
		        }
		    }
		    
		   	// Ataque 
		    aux1 = checa_casa(1,1);
		    aux2 = checa_casa(2,2);
		    aux3 = checa_casa(3,3);
		    if ((aux2 == strComputador) && (aux1 == "") && (aux3 == ""))
		    {
		        aux = checa_casa(1,1);
		    	if (aux == "")
		    	{
		       	    domina_casa(1,1,strComputador);
		       	    return 0;
		        }
		    }
		    
		    aux1 = checa_casa(3,1);
		    aux3 = checa_casa(1,3);
		    if ((aux2 == strComputador) && (aux1 == "") && (aux3 == ""))
		    {
		        aux = checa_casa(3,1);
		    	if (aux == "")
		    	{
		           	domina_casa(3,1,strComputador);
		           	return 0;
		        }
		    }
		     
			aux1 = checa_casa(1,1);
			if (aux1 == strComputador)
			{
				aux2 = checa_casa(2,2);
				aux3 = checa_casa(3,3);
				if ((aux3 == "") && (aux2 == ""))
				{
					aux1 = checa_casa(3,2);
					aux2 = checa_casa(1,2);
					if ((aux1 != strUsuario) && (aux2 != strUsuario))
					{
						aux1 = checa_casa(2,1);
						aux2 = checa_casa(2,3);
						if ((aux1 != strUsuario) && (aux2 != strUsuario))
						{
		                    aux = checa_casa(3,3);
		                   	if (aux == "")
		                    {
		                        domina_casa(3,3,strComputador);
							    return 0;
		                    }
		                }
					}
		        }
			}
			
			aux1 = checa_casa(2,3);
			aux2 = checa_casa(2,1);
			if ((aux1 == aux2) && (aux1 == strUsuario))
			{
				if ((checa_casa(1,1) == "") && (checa_casa(1,3) == "") && (checa_casa(3,1) == "") && (checa_casa(3,3) == ""))
				{
					domina_casa(1,1,strComputador);
					return 0;
				}
			}
		
		   	for (coluna = 1; coluna <= 3; coluna++)
		   	{
		        aux1 = checa_casa(1,coluna);
		        aux2 = checa_casa(2,coluna);
		        aux3 = checa_casa(3,coluna);
		        if ((aux1 == strComputador) && (aux2 == "") && (aux3 == ""))
		        {
		      	    aux = checa_casa(2,coluna);
		    	    if (aux == "")
		    	    {
		               	domina_casa(2,coluna,strComputador);
		               	return 0;
		            }
		        }
		        if ((aux2 == strComputador) && (aux1 == "") && (aux3 == ""))
		        {
		      	    aux = checa_casa(1,coluna);
		    	    if (aux == "")
		    	    {
		               	domina_casa(1,coluna,strComputador);
		           	    return 0;
		            }
		        }
		        if ((aux3 == strComputador) && (aux1 == "") && (aux2 == ""))
		        {
		      	    aux = checa_casa(1,coluna);
		    	    if (aux == "")
		    	    {
		               	domina_casa(1,coluna,strComputador);
		               	return 0;
		            }
		        }
		        
		        aux1 = checa_casa(coluna,1);
		        aux2 = checa_casa(coluna,2);
		        aux3 = checa_casa(coluna,3);
		        if ((aux1 == strComputador) && (aux2 == "") && (aux3 == ""))
		        {
		      	    aux = checa_casa(coluna,2);
		    	    if (aux == "")
		    	    {
		               	domina_casa(coluna,2,strComputador);
		               	return 0;
		            }
		        }
		        if ((aux2 == strComputador) && (aux1 == "") && (aux3 == ""))
		        {
		      	    aux = checa_casa(coluna,1);
		    	    if (aux == "")
		    	    {
		   	            domina_casa(coluna,1,strComputador);
		           	    return 0;
		            }
		        }
		        if ((aux3 == strComputador) && (aux1 == "") && (aux2 == ""))
		        {
		      	    aux = checa_casa(coluna,1);
		    	    if (aux == "")
		    	    {
		           	    domina_casa(coluna,1,strComputador);
		           	    return 0;
		            }
		        }
		    }
		
		    for (coluna = 1; coluna <= 3; coluna++)
		    {
		        aux1 = checa_casa(1,coluna);
		        aux2 = checa_casa(2,coluna);
		        aux3 = checa_casa(3,coluna);
		        if (aux1 == "")
		        {
		            domina_casa(1,coluna,strComputador);
		            return 0;
		        }
		        else if (aux2 == "")
		        {
		            domina_casa(2,coluna,strComputador);
		            return 0;
		        } 
		        else if (aux3 == "")
		        {
		            domina_casa(3,coluna,strComputador);
		            return 0;
		        }
		            
		        aux1 = checa_casa(coluna,1);
		        aux2 = checa_casa(coluna,2);
		        aux3 = checa_casa(coluna,3);
		        if (aux1 == "")
		        {
		            domina_casa(coluna,1,strComputador);
		            return 0;
		        }
		        else if (aux2 == "")
		        {
		            domina_casa(coluna,2,strComputador);
		            return 0;
		        }
		        else if (aux3 == "")
		        {
		            domina_casa(coluna,3,strComputador);
		            return 0;
		        } 
		    }
		
			return 1;
		}

        private bool verificaSedeuvelha()
        {
            if (btna1.Text != "" && btna2.Text != "" && btna3.Text != "" &&
                btnb1.Text != "" && btnb2.Text != "" && btnb3.Text != "" &&
                btnc1.Text != "" && btnc2.Text != "" && btnc3.Text != "")
            {
                return true;
            }
            return false;
        }


       
        private void CpuJoga()
        {
            dificuldade = Convert.ToString(comboBox1.SelectedItem);
            switch (dificuldade) 
            {
                case "Facil":
                    jogoFacil();
                    break;
                case "Medio":
                    jogoMedio();
                    break;
                case "HARD":
                    jogoDificil();
                    break;
            }
            checarJogo();

        }
        
        private void btnreiniciar_Click(object sender, EventArgs e)
        {
            reiniciarJogo();
        }

        private void checarJogo()
        {
            string ganhador = verificaGanhador();
            if (ganhador.Equals(strUsuario))
            {
                contvitoriasUsuario++;
                exibirContagem();
                MessageBox.Show("Você ganhou!");
                reiniciarJogo();
            }
            else
            {
                if (ganhador.Equals(strComputador))
                {
                    contvitoriasCpu++;
                    exibirContagem();
                    MessageBox.Show("Você perdeu!");
                    reiniciarJogo();
                }
                else
                {
                    if (verificaSedeuvelha())
                    {
                        contVelhas++;
                        exibirContagem();
                        MessageBox.Show("Deu velha!");
                        reiniciarJogo();
                    }
                    else
                    {
                        if (vezdaCpu)
                            CpuJoga();
                    }
                }
            }
        }

        private string verificaGanhador()
        {
            //coluna 1
            if (btna1.Text == btna2.Text && btna2.Text == btna3.Text && btna3.Text != string.Empty)
            {
                return btna1.Text;
            }

            //coluna 2
            if (btnb1.Text == btnb2.Text && btnb2.Text == btnb3.Text && btnb3.Text != string.Empty)
            {
                return btnb1.Text;
            }

            //coluna 3
            if (btnc1.Text == btnc2.Text && btnc2.Text == btnc3.Text && btnc3.Text != string.Empty)
            {
                return btnc1.Text;
            }

            //linha 1
            if (btna1.Text == btnb1.Text && btnb1.Text == btnc1.Text && btnc1.Text != string.Empty)
            {
                return btna1.Text;
            }

            //linha 2
            if (btna2.Text == btnb2.Text && btnb2.Text == btnc2.Text && btnc2.Text != string.Empty)
            {
                return btna2.Text;
            }

            //linha 3
            if (btna3.Text == btnb3.Text && btnb3.Text == btnc3.Text && btnc3.Text != string.Empty)
            {
                return btna3.Text;
            }

            //diagonais

            if (btna1.Text == btnb2.Text && btnb2.Text == btnc3.Text && btnc3.Text != string.Empty)
            {
                return btna1.Text;
            }

            if (btna3.Text == btnb2.Text && btnb2.Text == btnc1.Text && btnc1.Text != string.Empty)
            {
                return btna3.Text;
            }

            return "";
        }

        private bool marcaCasa(Button btnCasa, string caractere)
        {
            if (btnCasa.Text.Equals(""))
            {
                /* remove o botão da lista de casas disponíveis */
                listaCasasVazias.Remove(btnCasa);
                btnCasa.Text = caractere;

                if (caractere.Equals("O"))
                {
                    btnCasa.ImageIndex = 1;
                }
                else
                {
                    btnCasa.ImageIndex = 0;
                }
                
                if (caractere == strUsuario)
                {
                    /* registra as últimas escolhas do usuário */
                    if (ultimaSelecaoUsuario == null)
                    {
                        ultimaSelecaoUsuario = btnCasa;
                    }
                    else
                    {
                        penultimaSelecaoUsuario = ultimaSelecaoUsuario;
                        ultimaSelecaoUsuario = btnCasa;
                    }
                }

                return true;
            }
            return false;
        }

        private void btnb2_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text==String.Empty)
            {
                Button btn = (Button)sender;
                marcaCasa(btn, strUsuario);
                //((Button)sender).Text = strUsuario;

                vezdaCpu = true;
                checarJogo();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            inicializar();
            comboBox1.SelectedIndex = 0;
        }

    }
}
