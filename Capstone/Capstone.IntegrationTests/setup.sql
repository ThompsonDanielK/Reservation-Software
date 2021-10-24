-- Put steps here to set up your database in a default good state for testing

DELETE FROM reservation
DELETE FROM space
DELETE FROM category_venue
DELETE FROM venue
DELETE FROM city
DELETE FROM category
DELETE FROM state

INSERT INTO state
	(abbreviation, name)
VALUES
	('OH', 'OHIO')

SET IDENTITY_INSERT city ON

INSERT INTO city
	(id, name, state_abbreviation)
VALUES
	(1, 'Flavortown', 'OH')

SET IDENTITY_INSERT city OFF

SET IDENTITY_INSERT venue ON

INSERT INTO venue
	(id, name, city_id, description)
VALUES
	(1, 'The Lounge', 1, 'The cool place to be')

INSERT INTO venue
	(id, name, city_id, description)
VALUES
	(2, 'The Arena', 1, 'The dangerous place to be')

SET IDENTITY_INSERT venue OFF

SET IDENTITY_INSERT space ON

INSERT INTO space	
	(id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy)
VALUES
	(1, 1, 'The Cool Zone', 1, 6, 10, 1000, 20)

INSERT INTO space	
	(id, venue_id, name, is_accessible, daily_rate, max_occupancy)
VALUES
	(2, 1, 'The Tropic Zone', 0, 250, 30)

INSERT INTO space	
	(id, venue_id, name, is_accessible, daily_rate, max_occupancy)
VALUES
	(3, 2, 'The Crazy Zone', 0, 1000, 50)

SET IDENTITY_INSERT space OFF

SET IDENTITY_INSERT reservation ON

INSERT INTO reservation
	(reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for)
VALUES
	(1, 1, 1, '2021-01-01', '2021-01-05', 'Guy Fieri')

SET IDENTITY_INSERT reservation OFF


SET IDENTITY_INSERT category ON

INSERT INTO category
	(id, name)
VALUES
	(1, 'Meme')

SET IDENTITY_INSERT category OFF

INSERT INTO category_venue
	(venue_id, category_id)
VALUES
	(1, 1)