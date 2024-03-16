import socket
import threading
import json
import pygame

i = 0
received_messages = {}

def receive_messages(server_ip, server_port):
    global i, received_messages
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
        server_address = (server_ip, server_port)
        print(f"Connexion au serveur {server_address}")
        sock.connect(server_address)
        try:
            while True:
                data = sock.recv(1024)
                if data:
                    received_messages = json.loads(data.decode())
                    i = int(received_messages["value"])
                    print("Message reçu:", received_messages["value"])
                else:
                    print("Connexion fermée par le serveur.")
                    break
        except Exception as e:
            print(f"Erreur lors de la réception des messages : {e}")
        finally:
            print("Fermeture du socket")
            sock.close()

SERVER_IP = '127.0.0.1'
SERVER_PORT = 2345
#receive_messages(SERVER_IP, SERVER_PORT)

def run():
    pygame.init()

    screen_width = 640
    screen_height = 480
    screen = pygame.display.set_mode((screen_width, screen_height))

    black = (0, 0, 0)
    white = (255, 255, 255)

    font = pygame.font.Font(None, 74)


    running = True

    clock = pygame.time.Clock()

    client = threading.Thread(target=receive_messages, args=(SERVER_IP, SERVER_PORT))
    client.start()

    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

        screen.fill(black)

        text = font.render("Value: "+str(i), True, white)
        screen.blit(text, (screen_width // 2 - text.get_width() // 2, screen_height // 2 - text.get_height() // 2))

        pygame.display.flip()

        clock.tick(60)

    pygame.quit()

run()