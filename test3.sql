select 
	name, 
	family,
	thing: name.family,
	age: birthdate
from 
	Organization 
where 
	name = 'chalmers' 
	and 
	id = '4'