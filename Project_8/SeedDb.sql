USE HotelDb;
GO

INSERT INTO Rooms (RoomNumber, RoomType, PricePerNight, IsAvailable, Description, ImageUrl)
VALUES 
('101', 'Standart Oda', 1200.00, 1, 'Konforlu çift kişilik yatak, şehir manzaralı çalışma masası, ücretsiz Wi-Fi ve geniş banyo alanı.', '/images/room_standard.jpg'),
('102', 'Standart Oda', 1200.00, 1, 'Konforlu çift kişilik yatak, modern dekorasyon, minibar, ücretsiz Wi-Fi ve oda servisi.', '/images/room_standard.jpg'),
('201', 'Deluxe Oda', 2500.00, 1, 'Geniş kral yatak, panoramik deniz manzaralı pencere, lüks banyo aksesuarları ve oturma alanı.', '/images/room_deluxe.jpg'),
('202', 'Deluxe Oda', 2500.00, 1, 'Geniş kral yatak, muhteşem günbatımı manzaralı balkon, özel kahve makinesi ve çalışma alanı.', '/images/room_deluxe.jpg'),
('301', 'Kral Dairesi (Suite)', 5000.00, 1, 'Ultra lüks kral dairesi. Ayrı geniş salon, jakuzili banyo, panoramik deniz ve şehir manzaralı teras.', '/images/room_suite.jpg');
GO
