SELECT TOP 5 
	s.id, s.name, s.daily_rate, s.max_occupancy, s.is_accessible, s.daily_rate * @days AS totalcost 
FROM 
	space s 
WHERE 
	NOT EXISTS (SELECT * FROM reservation r INNER JOIN space s ON s.id = r.space_id
            WHERE start_date BETWEEN @startdate AND DATEADD(day, @days, @startdate) 
            AND r.end_date BETWEEN @startdate AND DATEADD(day, @days, @startdate) 
            AND s.venue_id = @venueid)
            AND ((MONTH(@startdate) > s.open_from 
            AND MONTH(@startdate) < s.open_to) OR s.open_from IS NULL) 
            AND s.venue_id = @venueid 
GROUP BY 
	s.id, s.name, s.daily_rate, s.max_occupancy, s.is_accessible, s.daily_rate * @days


SELECT * FROM space s where venue_id= @venueid AND s.id NOT IN (SELECT s.id from reservation r JOIN space s on r.space_id = s.id WHERE s.venue_id=  @venueid AND r.end_date >= DATEADD(day, @days, @startdate) AND r.start_date <= @startdate)
