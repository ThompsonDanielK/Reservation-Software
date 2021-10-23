SELECT TOP 5 
	s.id, s.name, s.daily_rate, s.max_occupancy, s.is_accessible, s.daily_rate * @days AS totalcost 
FROM 
	space s 
WHERE 
	NOT EXISTS (SELECT * FROM reservation 
            WHERE start_date BETWEEN @startdate AND DATEADD(day, @days, @startdate) 
            AND end_date BETWEEN @startdate AND DATEADD(day, @days, @startdate) 
            AND s.venue_id = @venueid)
            AND ((MONTH(@startdate) > s.open_from 
            AND MONTH(@startdate) < s.open_to) OR s.open_from IS NULL) 
            AND s.venue_id = @venueid 
GROUP BY 
	s.id, s.name, s.daily_rate, s.max_occupancy, s.is_accessible, s.daily_rate * @days