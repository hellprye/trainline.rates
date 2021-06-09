# Trainline Rates Assessment

## Things Id change or do differently

1. In CurrencyCalculator the calculated value is not rounded down or up as this is really dependant on business requirements and can differ depending on what is using it.
Theres even the potential that different services/apis will want to round different depending on the business function that is using this feature.

2. There is also a good argument case for making CurrencyCalculator an extension method of decimal instead of creating this class, but this implementation is still ok for MVP.

3. The Enums class could be changed. you could put the currency types into a database or as a json file in a shared content server that can be used across different apis.
In short Id see if we could have a single list of currency types that can be shared and easily modified if needed. This list does not need to have the exchange rates and is solely used for the type value.

## Other notes

1. I have created an infrastructure project and folders to represent entites and database classes if we need them. This is just by design and may or may not be needed depending on the scope of the MVP.
In this scenario its not needed but I felt it was best to show what a typical project structure should look like.