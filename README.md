# ITHögskolanPortProject

Port administration

◦We will build a port admin for a port with 64 port locations.
◦The harbor is visited by four different types of boats
◦Rowing boat
◦Motorboat
◦Sailboat
◦Freighters

Boat types-Rowing boat
◦Rowing boat
◦Takes up half a port space (two rowing boats can be accommodated in one place)
◦Properties
◦Identity number (random three letters with prefix “R-”, eg R-ABC)
◦Weight (100-300 kg)
◦Max speed (0-3 knots)
◦Unique property:
◦Maximum number of passengers (1-6 people)
Rowing boats stay in the harbor for 1 day, before returning to the sea.

Boat types-Motor boat
◦Motorboat
ArTakes up an entire berth (a motor boat can fit in one place)
◦Properties
DIdentity number (random three letters with prefix “M-”, eg M-ABC)
◦Weight (200-3000 kg)
AxMax speed (0-60 knots)
◦Unique property:
◦Number of horsepower (10-1000 hp)
◦Motorboats stay in the harbor for 3 days, before returning to the sea.


Boat types-Sailboat
◦ Sailboat
ArTakes up two full port locations
◦Properties
DIdentity number (random three letters with prefix “S-”, eg S-ABC)
◦Weight (800-6000 kg)
◦ Max speed (0-12 knots)
◦Unique property:
◦Boat Length (10-60 Feet)
◦Sailboats stay in the harbor for 4 days, before returning to the sea.


Boat types-Cargo ships
◦ Cargo ship
ArTakes up four full port locations
◦Properties
◦Identity number (random three letters with prefix “L-”, eg L-ABC)
◦Weight (3000-20000 kg)
◦ Max speed (0-20 knots)
◦Unique property:
◦ Load, number of containers on ship right now: 0-500
aCargo ships stay in the port for 6 days, before returning to the sea.


Conditions
◦ Every day 5 random boats arrive. (Allows you to easily change the value)
◦Boats are placed in the port as efficiently as possible, so there are as few gaps as possible
◦The rowing boats must be placed in pairs, at the same port.
◦No boat may be moved after they have been placed
◦As the boats leave the port, gaps arise, which incoming boats should primarily fill
◦The port boat stocks should be continuously saved as a file, so the application can start as, without the boats disappearing.
◦A “day” is either, for example, 5 seconds, or the days are switched with keystrokes or button-clicks.
◦If a boat cannot be placed in the harbor, the boat must be rejected. Keep track of how many they are.

Information to be displayed
◦ First and foremost, the occupancy of the 64 places must be reported, either as a list or graphically.
◦ In addition, the properties of the boats must be reported in column form, converted to metric units. These are identity number, weight, maximum speed (conversion from knots to km / h).
◦ The last column shows the boat's unique feature, such as the number of passengers for a rowing boat.
◦ In addition, a summary of the marina's stock should be displayed above or below the list:
◦ Number of rowing-sail-motor boats and cargo ships In the harbor
◦ Total weight In the port, ie the sum of the weight of all boats
◦ Average of the maximum speed of all boats
◦ Number of vacancies
◦ The number of show the boats (the boats that did not fit)
